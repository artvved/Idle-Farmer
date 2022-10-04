using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
    
        [SerializeField] private CapacityProgressBar capacityProgressBar;
        [SerializeField] private CoinsCounter coinsCounter;
    
        public override void InstallBindings()
        {
        
            Container.Bind<CapacityProgressBar>()
                .FromInstance(capacityProgressBar)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<CoinsCounter>()
                .FromInstance(coinsCounter)
                .AsSingle()
                .NonLazy();
        }
    }
}