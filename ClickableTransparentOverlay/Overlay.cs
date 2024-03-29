﻿namespace ClickableTransparentOverlay
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ImGuiNET;
    using SixLabors.ImageSharp.PixelFormats;
    using Veldrid;
    using Veldrid.ImageSharp;
    using Veldrid.Sdl2;
    using Veldrid.StartupUtilities;

    /// <summary>
    /// A class to create clickable transparent overlay.
    /// </summary>
    public abstract class Overlay : IDisposable
    {
        private const string DefaultWindowName = "Overlay";
        private readonly SDL_WindowFlags windowFlags =
            SDL_WindowFlags.Borderless 
            | SDL_WindowFlags.AlwaysOnTop  //comment here if U need debug imgui
            | SDL_WindowFlags.SkipTaskbar;

        private readonly Dictionary<string, Texture> loadedImages = new();
        private readonly string windowTitle;

        private volatile Sdl2Window window;
        private GraphicsDevice graphicsDevice;
        private CommandList commandList;
        private ImGuiController imController;

        private Thread renderThread;
        private volatile CancellationTokenSource cancellationTokenSource;
        private volatile bool overlayIsReady;

        private bool replaceFont = false;
        private ushort[] fontCustomGlyphRange;
        private string fontPathName;
        private float fontSize;
        private FontGlyphRangeType fontLanguage;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        public Overlay() : this(DefaultWindowName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="windowTitle">
        /// Title of the window created by the overlay
        /// </param>
        public Overlay(string windowTitle) : this(windowTitle, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="DPIAware">
        /// should the overlay scale with windows scale value or not.
        /// </param>
        public Overlay(bool DPIAware) : this(DefaultWindowName, DPIAware)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="windowTitle">
        /// Title of the window created by the overlay
        /// </param>
        /// <param name="DPIAware">
        /// should the overlay scale with windows scale value or not.
        /// </param>
        public Overlay(string windowTitle, bool DPIAware)
        {
            this.windowTitle = windowTitle;
            if (DPIAware)
            {
                NativeMethods.SetProcessDPIAware();
            }
        }

        #endregion

        /// <summary>
        /// Starts the overlay
        /// </summary>
        /// <returns>A Task that finishes once the overlay window is ready</returns>
        public async Task Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            renderThread = new Thread(async () =>
            {
                window = new Sdl2Window(windowTitle, 0, 0, 2560, 1440, windowFlags, false);
                graphicsDevice = VeldridStartup.CreateGraphicsDevice(window,
                    new GraphicsDeviceOptions(false, null, true),
                    GraphicsBackend.Direct3D11);
                commandList = graphicsDevice.ResourceFactory.CreateCommandList();
                imController = new ImGuiController(
                    graphicsDevice,
                    window.Width,
                    window.Height);
                window.Resized += () =>
                {
                    graphicsDevice.MainSwapchain.Resize((uint)window.Width, (uint)window.Height);
                    imController.WindowResized(window.Width, window.Height);
                };

                NativeMethods.InitTransparency(window.Handle);
                NativeMethods.SetOverlayClickable(window.Handle, false);
                imController.Start();
                if (!overlayIsReady)
                {
                    overlayIsReady = true;
                }

                PostStart();
                await RunInfiniteLoop(cancellationTokenSource.Token);
            });
            
            renderThread.Start();
            await WaitHelpers.SpinWait(() => overlayIsReady);
        }

        /// <summary>
        /// Starts the overlay and waits for the overlay window to be closed.
        /// </summary>
        /// <returns>A task that finishes once the overlay window closes</returns>
        public virtual async Task Run()
        {
            if (!overlayIsReady)
            {
                await Start();
            }

            await WaitHelpers.SpinWait(() => !window.Exists);
        }

        /// <summary>
        /// Infinitely calls the Render task until the overlay closes.
        /// </summary>
        private async Task RunInfiniteLoop(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            while (window.Exists && !cancellationToken.IsCancellationRequested)
            {
                InputSnapshot snapshot = window.PumpEvents();
                if (!window.Exists)
                {
                    Close();
                    break;
                }

                var deltaSeconds = (float)stopwatch.ElapsedTicks / Stopwatch.Frequency;
                stopwatch.Restart();
                imController.Update(deltaSeconds, snapshot, window.Handle);
                
                await Render();

                commandList.Begin();
                commandList.SetFramebuffer(graphicsDevice.MainSwapchain.Framebuffer);
                commandList.ClearColorTarget(0, new RgbaFloat(0.00f, 0.00f, 0.00f, 0.00f));
                imController.Render(graphicsDevice, commandList);
                commandList.End();
                graphicsDevice.SubmitCommands(commandList);
                graphicsDevice.SwapBuffers(graphicsDevice.MainSwapchain);
                ReplaceFontIfRequired();
            }

            if (window.Exists)
                window.Close();
        }

        /// <summary>
        /// Abstract Task for creating the UI.
        /// </summary>
        /// <returns>Task that finishes once per frame</returns>
        protected abstract Task Render();

        /// <summary>
        /// Steps to execute after the overlay has fully initialized.
        /// </summary>
        protected virtual void PostStart() { }

        /// <summary>
        /// Replaces the ImGui font with another one.
        /// </summary>
        /// <param name="pathName">pathname to the TTF font file.</param>
        /// <param name="size">font size to load.</param>
        /// <param name="language">supported language by the font.</param>
        /// <returns>true if the font replacement is valid otherwise false.</returns>
        public bool ReplaceFont(string pathName, int size, FontGlyphRangeType language)
        {
            if (!File.Exists(pathName))
            {
                return false;
            }

            fontPathName = pathName;
            fontSize = size;
            fontLanguage = language;
            replaceFont = true;
            fontCustomGlyphRange = null;
            return true;
        }

        /// <summary>
        /// Replaces the ImGui font with another one.
        /// </summary>
        /// <param name="pathName">pathname to the TTF font file.</param>
        /// <param name="size">font size to load.</param>
        /// <param name="glyphRange">custom glyph range of the font to load. Read <see cref="FontGlyphRangeType"/> for more detail.</param>
        /// <returns>>true if the font replacement is valid otherwise false.</returns>
        public bool ReplaceFont(string pathName, int size, ushort[] glyphRange)
        {
            if (!File.Exists(pathName))
            {
                return false;
            }

            fontPathName = pathName;
            fontSize = size;
            fontCustomGlyphRange = glyphRange;
            replaceFont = true;
            return true;
        }

        /// <summary>
        /// Safely Closes the Overlay.
        /// </summary>
        public virtual void Close()
        {
            cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Gets or sets the position of the overlay window.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(window.X, window.Y);
            }
            set
            {
                Sdl2Native.SDL_SetWindowPosition(window.SdlWindowHandle, value.X, value.Y);
            }
        }

        /// <summary>
        /// Gets the number of displays available on the computer.
        /// </summary>
        public static int NumberVideoDisplays
        {
            get
            {
                return Sdl2Native.SDL_GetNumVideoDisplays();
            }
        }

        /// <summary>
        /// Gets the monitor bounds based on the monitor number.
        /// </summary>
        /// <param name="num">Monitor number starting from 0.</param>
        /// <returns>monitor bounds in case of valid monitor number
        /// otherwise current overlay window bounds.</returns>
        public Rectangle GetDisplayBounds(int num)
        {
            int numDisplays = NumberVideoDisplays;
            if ( num >= numDisplays || num < 0)
            {
                return new Rectangle(Position, Size);
            }

            SDL2Functions.SDL_GetDisplayBounds(num, out Rectangle bounds);
            return bounds;
        }

        /// <summary>
        /// Gets or sets the size of the overlay window.
        /// </summary>
        public Point Size
        {
            get
            {
                return new Point(window.Width, window.Height);
            }
            set
            {
                Sdl2Native.SDL_SetWindowSize(window.SdlWindowHandle, value.X, value.Y);
            }
        }

        /// <summary>
        /// Adds the image to the Graphic Device as a texture.
        /// Then returns the pointer of the added texture. It also
        /// cache the image internally rather than creating a new texture on every call,
        /// so this function can be called multiple times per frame.
        /// </summary>
        /// <param name="filePath">Path to the image on disk.</param>
        /// <param name="handle">output pointer to the image in the graphic device.</param>
        /// <param name="width">width of the loaded texture.</param>
        /// <param name="height">height of the loaded texture.</param>
        public void AddOrGetImagePointer(
            string filePath,
            out IntPtr handle,
            out uint width,
            out uint height)
        {
            if (!loadedImages.TryGetValue(filePath, out Texture texture))
            {
                ImageSharpTexture imgSharpTexture = new(filePath);
                texture = imgSharpTexture.CreateDeviceTexture(graphicsDevice, graphicsDevice.ResourceFactory);
                loadedImages.Add(filePath, texture);
            }

            width = texture.Width;
            height = texture.Height;
            handle = imController.GetOrCreateImGuiBinding(graphicsDevice.ResourceFactory, texture);
        }

        /// <summary>
        /// Adds the image to the Graphic Device as a texture.
        /// Then returns the pointer of the added texture. It also
        /// cache the image internally rather than creating a new texture on every call,
        /// so this function can be called multiple times per frame.
        /// </summary>
        /// <param name="name">user friendly name given to the image.</param>
        /// <param name="image">image.</param>
        /// <param name="mipmap">
        /// a value indicating whether to create mipmap or not.
        /// For more info, read <see cref="ImageSharpTexture"/> code.
        /// </param>
        /// <param name="srgb">
        /// a value indicating whether pixel format is srgb or not.
        /// For more info, read <see cref="ImageSharpTexture"/> code.
        /// </param>
        /// <param name="handle">output pointer to the image in the graphic device.</param>
        /// <param name="width">width of the loaded texture.</param>
        /// <param name="height">width of the loaded texture.</param>
        public void AddOrGetImagePointer(
            string name,
            SixLabors.ImageSharp.Image<Rgba32> image,
            bool mipmap,
            bool srgb,
            out IntPtr handle,
            out uint width,
            out uint height)
        {
            if (!loadedImages.TryGetValue(name, out Texture texture))
            {
                ImageSharpTexture imgSharpTexture = new(image, mipmap, srgb);
                texture = imgSharpTexture.CreateDeviceTexture(graphicsDevice, graphicsDevice.ResourceFactory);
                loadedImages.Add(name, texture);
            }

            width = texture.Width;
            height = texture.Height;
            handle = imController.GetOrCreateImGuiBinding(graphicsDevice.ResourceFactory, texture);
        }

        /// <summary>
        /// Removes the image from the Overlay.
        /// </summary>
        /// <param name="name">
        /// name or pathname which was used to
        /// add the image in the first place.
        /// </param>
        /// <returns>true if image is removed otherwise false.</returns>
        public bool RemoveImage(string name)
        {
            if (loadedImages.Remove(name, out Texture texture))
            {
                imController.RemoveImGuiBinding(texture);
                texture.Dispose();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Free all resources acquired by the overlay.
        /// </summary>
        public virtual void Dispose()
        {
            if (renderThread.IsAlive)
            {
                Close();
            }

            graphicsDevice.WaitForIdle();
            RemoveAllImages();
            imController.Dispose();
            commandList.Dispose();
            graphicsDevice.WaitForIdle();
            graphicsDevice.Dispose();
        }

        private void RemoveAllImages()
        {
            var images = loadedImages.Keys.ToArray();
            for (int i = 0; i < images.Length; i++)
            {
                RemoveImage(images[i]);
            }
        }

        private void ReplaceFontIfRequired()
        {
            if (replaceFont)
            {
                var io = ImGui.GetIO();
                io.Fonts.Clear();
                unsafe
                {
                    var config = ImGuiNative.ImFontConfig_ImFontConfig();
                    if (fontCustomGlyphRange == null)
                    {
                        switch (fontLanguage)
                        {
                            case FontGlyphRangeType.English:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesDefault());
                                break;
                            case FontGlyphRangeType.ChineseSimplifiedCommon:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesChineseSimplifiedCommon());
                                break;
                            case FontGlyphRangeType.ChineseFull:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesChineseFull());
                                break;
                            case FontGlyphRangeType.Japanese:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesJapanese());
                                break;
                            case FontGlyphRangeType.Korean:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesKorean());
                                break;
                            case FontGlyphRangeType.Thai:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesThai());
                                break;
                            case FontGlyphRangeType.Vietnamese:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesVietnamese());
                                break;
                            case FontGlyphRangeType.Cyrillic:
                                io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, io.Fonts.GetGlyphRangesCyrillic());
                                break;
                            default:
                                throw new Exception("Font Glyph Range not supported.");
                        }
                    }
                    else
                    {
                        fixed (ushort* p = &fontCustomGlyphRange[0])
                        {
                            io.Fonts.AddFontFromFileTTF(fontPathName, fontSize, config, new IntPtr(p));
                        }
                    }

                    imController.RecreateFontDeviceTexture(graphicsDevice);
                    ImGuiNative.ImFontConfig_destroy(config);
                }

                replaceFont = false;
            }
        }
    }
}
