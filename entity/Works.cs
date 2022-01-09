namespace devrating.entity;

public interface Works
{
    InsertWorkOperation InsertOperation();
    GetWorkOperation GetOperation();
    ContainsWorkOperation ContainsOperation();
}
