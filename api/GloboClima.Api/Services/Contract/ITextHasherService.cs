namespace GloboClima.Api.Services.Contract;

public interface ITextHasherService
{
    public string Hash(string plainText);
    public bool Verify(string textHash, string plainText);
}