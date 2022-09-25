using UI;
using UnityEngine;
using Zenject;


public class UIInstaller : MonoInstaller
{
    
    [SerializeField] private CapacityProgressBar capacityProgressBar;
    
    public override void InstallBindings()
    {
        
        Container.Bind<CapacityProgressBar>()
            .FromInstance(capacityProgressBar)
            .AsSingle()
            .NonLazy();
    }
}