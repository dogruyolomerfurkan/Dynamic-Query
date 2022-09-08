namespace WebAPI.Entities;

public class QueryProperty
{
    /// <summary>
    /// Değişken adı
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// Değişken tipi
    /// </summary>
    public string FieldType { get; set; }

    /// <summary>
    /// Değişken Değeri
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Karşılaştırma Operatörü
    /// </summary>
    public string ComparisonOperator { get; set; }
}