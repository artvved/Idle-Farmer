using UnityEngine;
using Zenject;


public class DropSpawnManagerInstaller : MonoInstaller
{

   
    [SerializeField] private SpawnManager spawnManager;
    
    public override void InstallBindings()
    {
        
        Container.Bind<SpawnManager>()
            .FromInstance(spawnManager)
            .AsSingle()
            .NonLazy();
    }
}