namespace Tool.Compet.AutoDI;

/// <summary>
/// Register the class as scoped with interfaces.
/// </summary>
public class RegisterAsSingletonWithInterfaces : AutoDIRegistrationAttribute {
	public static readonly string FullName = typeof(RegisterAsSingletonWithInterfaces).FullName!;

	public RegisterAsSingletonWithInterfaces() {
		this.serviceLifetime = ServiceLifetime.Singleton;
	}
}
