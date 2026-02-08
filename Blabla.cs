ChipReader chippie = new ChipReader();
chippie.InsertCard();
string credential = chippie.ReadCredentials();

NFCReader fonie = new NFCReader();
fonie.HoldNear(10);
string foniecredential = fonie.ReadCredentials();

CryptoTerminal crippie = new CryptoTerminal("10",fonie);
crippie.ProcessPayment(2.22m);

// dit is een wijziging 2

// dit is een wijziging.

// dit is een wijziging 3

CreditCardTerminal creditcard = new CreditCardTerminal("100", chippie);
creditcard.ProcessPayment(100);
creditcard.ProcessPayment(9.99m);

creditcard.PrintReceipt();
crippie.PrintReceipt();
public interface IInputDevice
{
    public void Connect();
    public string ReadCredentials();
}

public class ChipReader : IInputDevice
{
    private bool _isCardInserted;
    private string _encryptionKey;
    public void InsertCard()
    {
        _isCardInserted = true;
    }
    public void EjectCard()
    {
        _isCardInserted = false;
    }

    public void Connect()
    {
        
    }

    public string ReadCredentials()
    {
        if (_isCardInserted == true)
        {
            return "ajdfopiajd0-a90";
        }
        return "ERROR: Geen kaart";
    }
}

public class NFCReader : IInputDevice
{
    private int _signalStrength;
    public void HoldNear(int distance)
    {
        _signalStrength = Math.Max(100 - distance * 3, 0);
    }
    public void Connect()
    {
        
    }

    public string ReadCredentials()
    {
        if (_signalStrength > 50)
        {
            return "09ajv0a9jvj";
        }
        return "Error: Kaart te ver";
    }
}

public abstract class PaymentTerminal
{
    protected decimal _dailyTurnover;
    public string TerminalID;
    public IInputDevice InputDevice;
    public abstract void ProcessPayment(decimal amount);
    public void PrintReceipt()
    {
        Console.WriteLine($"De dagelijkse omzet is vandaag {_dailyTurnover}");
    }
    protected PaymentTerminal(string ID, IInputDevice device)
    {
       TerminalID = ID;
       InputDevice = device; 
    }
    
}

public class CreditCardTerminal : PaymentTerminal
{
    private decimal _bankConnectionFee = 0.4m;
    public override void ProcessPayment(decimal amount)
    {
        string credentials = InputDevice.ReadCredentials();
        if (credentials.Contains("Error", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine("Betaling mislukt");
            return;
        }
        Console.WriteLine("Betaling geslaagd via Bank");
        _dailyTurnover = _dailyTurnover + amount - _bankConnectionFee;
    }
    public CreditCardTerminal(string ID, IInputDevice device) : base(ID, device)
    {
       
    }
}

public class CryptoTerminal : PaymentTerminal
{
    private decimal _exchangeRate = 0.00045m;
    public override void ProcessPayment(decimal amount)
    {
        decimal crypto = amount * _exchangeRate;
        _dailyTurnover += crypto;
    }
    public CryptoTerminal(string ID, IInputDevice device) : base(ID, device)
    {
        
    }
}
