namespace Shared
{
    public static class ConstantsDefaults
    {
    }
    public static class LoteStatus
    {
        public const string Received = "RECIBIDO";
        public const string Located = "UBICADO";
        public const string ToProcess = "POR_PROCESAR";
        public const string InProcess = "EN_PROCESO";
        public const string Verified = "VERIFICADO";
        public const string InReview = "EN_REVISION";
        public const string PartiallySupplied = "ABASTECIDO_PARCIAL";
        public const string Stocked = "ABASTECIDO";
    }

    public static class UserRoles
    {
        public const int Admin = 1;
        public const int Montacargas = 2;
        public const int OperarioApertura = 3;
        public const int Salvamento = 4;
        public const int Coordinador = 5;
        public const int ResponsableAbastecimiento = 6;
        public const int ResponsableEstación = 7;
    }

    public static class SupplyStatus
    {
        public static string Pending = "PENDIENTE";
        public const string InProgress = "EN_PROGRESO";
        public const string Completed = "COMPLETADO";
    }
}
