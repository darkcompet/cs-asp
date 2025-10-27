namespace Tool.Compet.AutoDI;

/// <summary>
/// Register the class as scoped with interfaces.
/// </summary>
public class RegisterAsTransientWithInterfaces : AutoDIRegistrationAttribute {
	public static readonly string FullName = typeof(RegisterAsTransientWithInterfaces).FullName!;

	public RegisterAsTransientWithInterfaces() {
		this.serviceLifetime = ServiceLifetime.Transient;
	}
}
