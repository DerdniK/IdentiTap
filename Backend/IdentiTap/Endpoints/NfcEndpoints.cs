namespace IdentTap.API.Endpoints;

public static class NfcEndpoints
{
    public static void MapNfcEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/nfc");

        group.MapGet("/{nfcUid}/public", (string nfcUid) => Results.Ok(new
        {
            NfcUid = nfcUid,
            BloodType = "O+",
            EmergencyContact = "+52 442 000 0000"
        })).AllowAnonymous();

        group.MapGet("/{nfcUid}/medical-records", (string nfcUid) => Results.Ok(new
        {
            NfcUid = nfcUid,
            ClinicalHistory = "Expediente médico cifrado...",
            Documents = new[] { "receta1.pdf", "analisis_sangre.pdf" }
        })).RequireAuthorization();
    }
}