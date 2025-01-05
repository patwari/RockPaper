
using Sound;
using Utils;


/**
This is the central DI, which contains references to all objects, which are useful for the game.

Please NOTE: The `Init.unity` scene > `di` prefab is a gameObject, just storing some managers as children. 
That has got very little to do with this static class.
Technically that gameObject is not connected to this static class. Just logically.
*/
public class DI {
    private DI() { }

    public static DI di { get; private set; } = new DI();

    public SoundManager soundManager { get; private set; }
    public DataSaver dataSaver { get; private set; }

    internal void SetSoundManager(SoundManager soundManager) => this.soundManager = soundManager;
    internal void SetDataSaver(DataSaver dataSaver) => this.dataSaver = dataSaver;
}