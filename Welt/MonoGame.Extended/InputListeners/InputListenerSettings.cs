namespace Welt.MonoGame.Extended.InputListeners
{
    public abstract class InputListenerSettings<T>
        where T : InputListener
    {
        internal abstract T CreateListener();
    }
}