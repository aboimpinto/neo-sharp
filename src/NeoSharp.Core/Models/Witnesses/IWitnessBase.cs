namespace NeoSharp.Core.Models.Witnesses
{
    public interface IWitnessBase : ISignable<ISignedWitnessBase>
    {
        byte[] InvocationScript { get; set; }

        byte[] VerificationScript { get; set; }
    }
}
