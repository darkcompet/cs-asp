namespace Tool.Compet.AutoDI;

/// <summary>
/// Register the class as scoped with interfaces.
/// </summary>
public class RegisterAsScopedWithInterfaces : AutoDIRegistrationAttribute {
	public static readonly string FullName = typeof(RegisterAsScopedWithInterfaces).FullName!;

	public RegisterAsScopedWithInterfaces() {
		this.serviceLifetime = ServiceLifetime.Scoped;
	}
}
