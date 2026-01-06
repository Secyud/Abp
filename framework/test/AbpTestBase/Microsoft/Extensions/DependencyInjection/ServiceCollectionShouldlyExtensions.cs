using Shouldly;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionShouldlyExtensions
{
    extension(IServiceCollection services)
    {
        public void ShouldContainTransient(Type serviceType, Type? implementationType = null)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(implementationType ?? serviceType);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
        }

        public void ShouldContainTransientImplementationFactory(Type serviceType)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBeNull();
            serviceDescriptor.ImplementationFactory.ShouldNotBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
        }

        public void ShouldContainSingleton(Type serviceType, Type? implementationType = null)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(implementationType ?? serviceType);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
        }

        public void ShouldContainScoped(Type serviceType, Type? implementationType = null)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(implementationType ?? serviceType);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
        }

        public void ShouldContain(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(implementationType);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(lifetime);
        }

        public void ShouldNotContainService(Type serviceType)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldBeNull();
        }
    }
}
