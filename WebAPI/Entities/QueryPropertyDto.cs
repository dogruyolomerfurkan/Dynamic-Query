namespace WebAPI.Entities;

public class QueryPropertyDto
{
    /// <summary>
    /// Sorgu içerikleri
    /// </summary>
    public ICollection<QueryProperty> QueryProperties { get; set; }

    /// <summary>
    /// Mantık Operatörü
    /// </summary>
    public string LogicalOperator { get; set; }
}