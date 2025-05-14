namespace Ins.ReqModels;

public class ModelAgreementsWithoutDateFinish
{
    public ModelInsPerson Person { get; set; }
    public ModelAnimal Animal { get; set; }
    public ModelApartment Apartment { get; set; }
    public DateTime StartDate { get; set; }
    public double Price{ get; set; }
    public string InsCompanyName { get; set; }
    public string TypeInsName { get; set; }
    
}
