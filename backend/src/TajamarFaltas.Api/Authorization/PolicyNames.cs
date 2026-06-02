namespace TajamarFaltas.Api.Authorization;

public static class PolicyNames
{
    public const string AlumnoOnly = "AlumnoOnly";
    public const string ProfesorOnly = "ProfesorOnly";
    public const string AdministradorOnly = "AdministradorOnly";
    public const string ProfesorOrAdministrador = "ProfesorOrAdministrador";
}