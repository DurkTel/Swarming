
/// <summary>
///事件枚举 
/// </summary>
///事件名称统一用大写规范 

public enum EventDefine
{
    /// <summary>
    /// 开启角色转换能力
    /// </summary>
    CHANGE_CHAR_TRIGGER,
    /// <summary>
    /// 风开启视角转换能力
    /// </summary>
    CHANGE_VISUAL_TRIGGER,
    /// <summary>
    /// 转换视觉状态
    /// </summary>
    CHANGE_VISUAL_STATE,
    /// <summary>
    /// 转换角色
    /// </summary>
    CHANGE_CHAR,
    /// <summary>
    /// 开始走路
    /// </summary>
    WALK,
    /// <summary>
    /// 跳跃结束
    /// </summary>
    JUMP_END,
    /// <summary>
    /// 开始跳跃
    /// </summary>
    JUMP,
    /// <summary>
    /// 滚动
    /// </summary>
    ROLL,
    /// <summary>
    /// 切换摄像机
    /// </summary>
    CHANGE_CAM,
    /// <summary>
    /// 机器人的启动
    /// </summary>
    ROBOT_OPEN,
    /// <summary>
    /// 保存游戏
    /// </summary>
    SAVE_GAME,
    /// <summary>
    /// 金币更新
    /// </summary>
    GOLDUPDATE,
    /// <summary>
    /// 生命更新
    /// </summary>
    LIFEUPDATE,


    //按键触发-------------------
    /// <summary>
    /// 互动键
    /// </summary>
    INTERACTION_KEY,
    /// <summary>
    /// 跳跃键
    /// </summary>
    JUMP_KEY,



    DIALOG,
    //关卡相关-------------------1
    ENTER_BROKEN_BRIDGE,
    FALL_ROCK_TRIGGER,
    PRESSURE_PLATE_TRIGGER,
    PULL_ROD_TRIGGER,
    WIND_POWER_TRIGGER,
    FIRE_TRIGGER_1,
    TRAIN_START,
    ENTER_PLATFORM,
    PLATFORM,
    PLATFORMERROR,
    OPENPUMICE,
    PUMICETRAP,
    FIRE_TIPS,

    //关卡相关-------------------2
    FLOAT_BOARD,
    BOARD_STOP,
    ROTPOWER,
    ROLL_1,
    ROLL_2,
    ROLL_STOP_1,
    ROLL_STOP_2,
    CONDUIT,
    CONDUITEX,
    SOKOBAN,
    SOKOBANEX,
    DRAWWORDENTER,
    ENTER_SOKOBAN,
    DRAWWORDEND,
    STARTPILLARPUZZLE,
    PILLARPUZZLE,
    FINALSWITCHUP,
    LETTER,

    /// <summary>
    /// 游戏结束
    /// </summary>
    GAME_END
}
