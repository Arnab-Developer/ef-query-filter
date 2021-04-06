# EF core global query filter

This is a demo application to show the usage of Entity Framework Core's global query filter.

What is global query filter? https://docs.microsoft.com/en-us/ef/core/querying/filters

Global query filter can be mentioned in the `OnModelCreating` method of `context` class.

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.SetQueryFilterOnAllEntities<ISchool>(s => s.SchoolId == SchoolId);
}
```

When this `context` class will be used then automatically filter will be applied before 
returning result.

Test has been written to show the output.

```c#
[Theory]
[InlineData(1)]
[InlineData(2)]
public void VerifyThatSchoolCanNotGetOtherSchoolsData(int schoolId)
{
    // code here...
}
```
