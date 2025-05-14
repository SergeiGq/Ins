namespace Ins.ReqModels;

public class ModelAgreements
{
    public ModelInsPerson Person { get; set; }
    public ModelAnimal Animal { get; set; }
    public ModelApartment Apartment { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public double Price{ get; set; }
    public string InsCompanyName { get; set; }
    public string TypeInsName { get; set; }
    
}