﻿namespace Stas.Utils;

public enum Opcode : byte {
    Unknown,
    Ping,
    Login,
    Message,
    LoginOK,
    Server_down,
    ServerReady,
    SetlockAtPoint,
    MouseLeftClick, 
    MouseRightClick,
    MouseLeftDown,
    MouseLeftUp,
    BuffUp,
    KeyDown,
    KeyUp,
    KeyPress,
    ShiftDown,
    ShiftUp,
    BotRoleList,
    SetRole,
    StartMoving,
    StopMoving,
    SavingAss,
    StopAll,
    StopWorking,
    UseMacros,
    UseFlare,
    OpenInBrowser,
    StartScreening,
    StopScreening,
    ScreenInfo,
    OneImageBuffer,
    ImageOver,
    Test,
    ScreenQ,
    MouseScroll,
    OpenTrade,
    Looting,
    GetPos,
    SetTarget,
    NewImage,
    BotInfo,
    Started,
    Stopped,
    CleareErr,
    ReloadMem,
    UpdateMap,
    NewMapItemsList,
    ItemsList,
    RestartUdp,
    ReloadMap,
    Transit,
    SetLeader,
    Log,
    ClearBadLoot,
    Hold,
    UseLoot,
    UseChest,
    UnHold,
    TpToLeader,
    Test2,
    SetCursorOnScreen,
    NewImagePart,
    PlaySound,
    ResetState, NavGo, FallowHard, Jump, LinkMe, ResurectCheckPoint, ResurectTown, UseTotem, Exit,
    FocusGP, SetUltim, UseTNT
}