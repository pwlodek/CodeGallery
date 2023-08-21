using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using System.Fabric;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Client;
using SampleActor.Interfaces;

namespace SampleActor
{
    class SampleActorService : ActorService
    {
        public SampleActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null) : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await base.RunAsync(cancellationToken);

            ISampleActor myActor = ActorProxy.Create<ISampleActor>(ActorId.CreateRandom(), new Uri("fabric:/ECommerce/SampleActorService"));

            var list = await myActor.GetValuesAsync(CancellationToken.None);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
