using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;
namespace ConsoleApp;
public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) =>
   deliveries.Where(x => x.PaymentId != null); //TODO: Завдання 1
    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries)
   => deliveries.Where(x => x.Status != DeliveryStatus.Cancelled && x.Status !=
   DeliveryStatus.Done); //TODO: Завдання 2
    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo>
   DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) =>
   deliveries.Where(y => y.ClientId.Equals(clientId)).Select(x => new
   DeliveryShortInfo
   {
       Id = x.Id,
       Status = x.Status,
       ClientId = clientId,
       ArrivalPeriod = x.ArrivalPeriod,
       CargoType = x.CargoType,
       StartCity = x.Direction.Origin.City,
       EndCity = x.Direction.Destination.City,
       LoadingPeriod = x.LoadingPeriod,
       Type = x.Type,
   }); //TODO: Завдання 3

    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have
    specified type
 /// </summary>
 public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery>
deliveries, string cityName, DeliveryType type) => deliveries.Where(x =>
x.Type == type && x.Direction.Origin.City == cityName); //TODO: Завдання 4

    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery>
   OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) =>
   deliveries.OrderBy(x => x.Status).ThenBy(x => x.LoadingPeriod.Start);//TODO:
    Завдання 5
 /// <summary>
 /// Count unique cargo types
 /// </summary>
 public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) =>
deliveries.DistinctBy(x => x.CargoType).Count(); //TODO: Завдання 6
    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int>
   CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) =>
   deliveries.GroupBy(x => x.Status).ToDictionary(x => x.Key, y =>
   y.Count());//TODO: Завдання 7
    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap
    between end of loading period and start of arrival period(calculate in
    minutes)
 /// </summary>
 public IEnumerable<AverageGapsInfo>
AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) => deliveries
 .Where(x => x.LoadingPeriod.End.HasValue &&
x.ArrivalPeriod.Start.HasValue)
 .GroupBy(x => new {
     StartCity = x.Direction.Origin.City,
     EndCity =
x.Direction.Destination.City
 })
 .Select(y => new AverageGapsInfo
 {
     StartCity = y.Key.StartCity,
     EndCity = y.Key.EndCity,
     AverageGap = y.Average(x => (x.ArrivalPeriod.Start.Value -
    x.LoadingPeriod.End.Value).TotalMinutes)
 });
    //TODO: Завдання 8
    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement,
   TOrderingKey>(IEnumerable<TElement> elements,
    Func<TElement, TOrderingKey> ordering,
    Func<TElement, bool>? filter = null,
    int countOnPage = 100,
    int pageNumber = 1) => elements
    .Where(filter ?? (_ => true))
    .OrderBy(ordering)
    .Skip((pageNumber - 1) * countOnPage)
    .Take(countOnPage);
    //TODO: Завдання 9
}