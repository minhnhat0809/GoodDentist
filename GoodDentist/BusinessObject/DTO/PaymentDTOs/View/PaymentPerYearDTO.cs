namespace BusinessObject.DTO.PaymentDTOs.View;

public class PaymentPerYearDTO
{
    public string Month { get; set; }
    
    public decimal Income { get; set; }

    public PaymentPerYearDTO(string Month, decimal Income)
    {
        this.Month = Month;
        this.Income = Income;
    }
}