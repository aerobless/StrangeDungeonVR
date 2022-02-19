namespace SixtyMeters.logic.interfaces.lifecycle
{
    public interface ITrackedLifecycle
    {
        
        public void RegisterDestructionListener(IDestructionListener destructionListener);
    }
}