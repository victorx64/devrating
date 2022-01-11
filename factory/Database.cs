namespace devrating.entity;

public interface Database
{
    DbInstance Instance();
    Entities Entities();
}
