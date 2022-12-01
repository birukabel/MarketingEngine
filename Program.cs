using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static TicketsConsole.Program;

/*

Let's say we're running a small entertainment business as a start-up. This means we're selling tickets to live events on a website. An email campaign service is what we are going to make here. We're building a marketing engine that will send notifications (emails, text messages) directly to the client and we'll add more features as we go.

Please, instead of debuging with breakpoints, debug with "Console.Writeline();" for each task because the Interview will be in Coderpad and in that platform you cant do Breakpoints.

*/

namespace TicketsConsole
{
    internal class Program
    {

        static void Main(string[] args)
        {
            /*

            1. You can see here a list of events, a customer object. Try to understand the code, make it compile. 

            2.  The goal is to create a MarketingEngine class sending all events through the constructor as parameter and make it print the events that are happening in the same city as the customer. To do that, inside this class, create a SendCustomerNotifications method which will receive a customer as parameter and will mock the Notification Service. Add this ConsoleWriteLine inside the Method to mock the service. Inside this method you can add the code you need to run this task correctly but you cant modify the console writeline: Console.WriteLine($"{customer.Name} from {customer.City} event {e.Name} at {e.Date}");

            3. As part of a new campaign, we need to be able to let customers know about events that are coming up close to their next birthday. You can make a guess and add it to the MarketingEngine class if you want to. So we still want to keep how things work now, which is that we email customers about events in their city or the event closest to next customer's birthday, and then we email them again at some point during the year. The current customer, his birthday is on may. So it's already in the past. So we want to find the next one, which is 23. How would you like the code to be built? We don't just want functionality; we want more than that. We want to know how you plan to make that work. Please code it.

            4. The next requirement is to extend the solution to be able to send notifications for the five closest events to the customer. The interviewer here can paste a method to help you, or ask you to search it. We will attach 2 different ways to calculate the distance. 

            // ATTENTION this row they don't tell you, you can google for it. In some cases, they pasted it so you can use it

            Option 1:
            var distance = Math.Abs(customerCityInfo.X - eventCityInfo.X) + Math.Abs(customerCityInfo.Y - eventCityInfo.Y);

            Option 2:
            private static int AlphebiticalDistance(string s, string t)
            {
                var result = 0;
                var i = 0;
                for(i = 0; i < Math.Min(s.Length, t.Length); i++)
                    {
                        // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                        result += Math.Abs(s[i] - t[i]);
                    }
                    for(; i < Math.Max(s.Length, t.Length); i++)
                    {
                        // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                        result += s.Length > t.Length ? s[i] : t[i];
                    }
                    
                    return result;
            } 

            Tips of this Task:
            Try to use Lamba Expressions. Data Structures. Dictionary, ContainsKey method.

            5. If the calculation of the distances is an API call which could fail or is too expensive, how will you improve the code written in 4? Think in caching the data which could be code it as a dictionary. You need to store the distances between two cities. Example:

            New York - Boston => 400 
            Boston - Washington => 540
            Boston - New York => Should not exist because "New York - Boston" is already stored and the distance is the same. 

            6. If the calculation of the distances is an API call which could fail, what can be done to avoid the failure? Think in HTTPResponse Answers: Timeoute, 404, 403. How can you handle that exceptions? Code it.

            7.  If we also want to sort the resulting events by other fields like price, etc. to determine whichones to send to the customer, how would you implement it? Code it.
            */

            var events = new List<Event>{
                new Event(1, "Phantom of the Opera", "New York", new DateTime(2023,12,23)),
                new Event(2, "Metallica", "Los Angeles", new DateTime(2023,12,02)),
                new Event(3, "Metallica", "New York", new DateTime(2023,12,06)),
                new Event(4, "Metallica", "Boston", new DateTime(2023,10,23)),
                new Event(5, "LadyGaGa", "New York", new DateTime(2023,09,20)),
                new Event(6, "LadyGaGa", "Boston", new DateTime(2023,08,01)),
                new Event(7, "LadyGaGa", "Chicago", new DateTime(2023,07,04)),
                new Event(8, "LadyGaGa", "San Francisco", new DateTime(2023,07,07)),
                new Event(9, "LadyGaGa", "Washington", new DateTime(2023,05,22)),
                new Event(10, "Metallica", "Chicago", new DateTime(2023,01,01)),
                new Event(11, "Phantom of the Opera", "San Francisco", new DateTime(2023,07,04)),
                new Event(12, "Phantom of the Opera", "Chicago", new DateTime(2024,05,15))
            };

            var customer = new Customer()
            {
                Id = 1,
                Name = "John",
                City = "New York",
                BirthDate = new DateTime(1995, 05, 10)
            };
            MarketingEngine me = new MarketingEngine(customer, events);
        }

        public class Event
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public DateTime Date { get; set; }

            public Event(int id, string name, string city, DateTime date)
            {
                this.Id = id;
                this.Name = name;
                this.City = city;
                this.Date = date;
            }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public DateTime BirthDate { get; set; }
        }


        /*-------------------------------------
        Coordinates are roughly to scale with miles in the USA
           2000 +----------------------+  
                |                      |  
                |                      |  
             Y  |                      |  
                |                      |  
                |                      |  
                |                      |  
                |                      |  
             0  +----------------------+  
                0          X          4000
        ---------------------------------------*/

        public class MarketingEngine
        {
            //public List<Event> lsEvent = new();
            public MarketingEngine(Customer customer, List<Event> lsEe)
            {
                Console.WriteLine(@"+++++++++++++Events happing in customer's city");
                GetClosestEventsForCustomersCity(customer, lsEe);
                Console.WriteLine(@"+++++++++++++Events happing near to customer's next birthday");
                GetEventsNearCustomersBirthDay(customer, lsEe);
                Console.WriteLine(@"+++++++++++++Five Events happing near to customer's city");
                GetFiveClosestEventsForCustomer(customer, lsEe);
                Console.WriteLine(@"+++++++++++++Five Events happing near to customer's city implemented with cache");
                GetFiveClosestEventsForCustomerWithCache(customer, lsEe);
                Console.WriteLine(@"+++++++++++++Five Events happing near to customer's city implemented with cache and HttpStatusCode");
                GetFiveClosestEventsForCustomerWithHttpStatusCode(customer, lsEe);
                Console.WriteLine(@"+++++++++++++Five Events happing near to customer's city again sorted via price");
                GetFiveClosestEventsForCustomerWithPrice(customer, lsEe);
            }

            public void GetClosestEventsForCustomersCity(Customer customer, List<Event> ls)
            {

                var ev = from x in ls
                         where x.City == customer.City
                         select x;
                foreach (var e in ev)
                {
                    SendCustomerNotifications(customer, e);
                }
            }

            public void GetEventsNearCustomersBirthDay(Customer customer, List<Event> ev)
            {
                Dictionary<Event, int> diEvents = new();
                DateTime nextBirthDay = new DateTime(2023, 05, 10);
                foreach (Event e in ev)
                {
                    if (e.Date < nextBirthDay)
                    {
                        continue;
                    }
                    int diff = 0;
                    if (e.Date > customer.BirthDate)
                    {
                        diff = (e.Date - nextBirthDay).Days;
                    }
                    else
                    {
                        diff = (nextBirthDay - e.Date).Days;
                    }
                    if (!diEvents.ContainsKey(e))
                    {
                        diEvents.Add(e, diff);
                    }
                }

                var x = (from y in diEvents
                         orderby y.Value
                         select y).Take(5);
                foreach (var item in x)
                {
                    Console.WriteLine("Event name = {0} Event difference = {1}", item.Key.Name, item.Value);
                    SendCustomerNotifications(customer, item.Key);
                }
            }

            public void SendCustomerNotifications(Customer customer, Event e)
            {
                Console.WriteLine($"{customer.Name} from {customer.City} event {e.Name} at {e.Date}");
            }

            public void GetFiveClosestEventsForCustomer(Customer customer, List<Event> events)
            {
                Dictionary<Event, int> dicFiveClosest = new();
                foreach (Event ev in events)
                {
                    int dist = GetDistance(customer.City, ev.City);
                    if (!dicFiveClosest.ContainsKey(ev))
                    {
                        dicFiveClosest.Add(ev, dist);
                    }
                }

                var x = (from y in dicFiveClosest
                         orderby y.Value
                         select y).Take(5);

                Console.WriteLine("5 closest events near customer are displayed as follows");
                foreach (var item in x)
                {
                    SendCustomerNotifications(customer, item.Key);
                }
            }

            public void GetFiveClosestEventsForCustomerWithCache(Customer customer, List<Event> events)
            {
                Dictionary<Event, int> dicFiveClosest = new();
                Dictionary<string, int> dicDistance = new();
                foreach (Event ev in events)
                {
                    int dist = 0;
                    if (dicDistance.ContainsKey(customer.City + '-' + ev.City))
                    {
                        dist = dicDistance[customer.City + '-' + ev.City];
                    }
                    else
                    {
                        dist = GetDistance(customer.City, ev.City);
                    }
                    if (!dicFiveClosest.ContainsKey(ev))
                    {
                        dicFiveClosest.Add(ev, dist);
                    }
                }

                var x = (from y in dicFiveClosest
                         orderby y.Value
                         select y).Take(5);

                Console.WriteLine("5 closest events near customer are displayed as follows");
                foreach (var item in x)
                {
                    SendCustomerNotifications(customer, item.Key);
                }
            }

            public int GetDistance(string fromCity, string toCity)
            {
                int result = 0;
                int minLength = Math.Min(fromCity.Length, toCity.Length);
                int maxLength = Math.Max(fromCity.Length, toCity.Length);
                int i = 0;
                for (; i < minLength; i++)
                {
                    result += Math.Abs(fromCity[i] - toCity[i]);
                }
                for (; i < maxLength; i++)
                {
                    result += fromCity.Length > toCity.Length ? Convert.ToInt32(fromCity[i]) : Convert.ToInt32(toCity[i]);
                }

                return result;
            }

            public void GetFiveClosestEventsForCustomerWithHttpStatusCode(Customer customer, List<Event> events)
            {
                if (customer.City.Length == 0 || events.Count == 0)
                { 
                    Console.WriteLine("Error while processing your request exiting with code {0}",HttpStatusCode.Forbidden);
                }
                Dictionary<Event, int> dicFiveClosest = new();
                Dictionary<string, int> dicDistance = new();
                try
                {
                    foreach (Event ev in events)
                    {
                        int dist = 0;
                        if (dicDistance.ContainsKey(customer.City + '-' + ev.City))
                        {
                            dist = dicDistance[customer.City + '-' + ev.City];
                        }
                        else
                        {
                            dist = GetDistance(customer.City, ev.City);
                        }
                        if (!dicFiveClosest.ContainsKey(ev))
                        {
                            dicFiveClosest.Add(ev, dist);
                        }
                    }

                    var x = (from y in dicFiveClosest
                             orderby y.Value
                             select y).Take(5);

                    Console.WriteLine("5 closest events near customer are displayed as follows");
                    foreach (var item in x)
                    {
                        SendCustomerNotifications(customer, item.Key);
                    }
                }
                catch
                {
                    Console.WriteLine("Error while processing your request exiting with code {0}", HttpStatusCode.InternalServerError);
                }
            }

            public void GetFiveClosestEventsForCustomerWithPrice(Customer customer, List<Event> events)
            {
                if (customer.City.Length == 0 || events.Count == 0)
                {
                    Console.WriteLine("Error while processing your request exiting with code {0}", HttpStatusCode.Forbidden);
                }

                Dictionary<Event, decimal> dicEventWithPrice = new();
                Dictionary<Event, int> dicFiveClosest = new();
                Dictionary<string, int> dicDistance = new();
                try
                {
                    foreach (Event ev in events)
                    {
                        int dist = 0;
                        if (dicDistance.ContainsKey(customer.City + '-' + ev.City))
                        {
                            dist = dicDistance[customer.City + '-' + ev.City];
                        }
                        else
                        {
                            dist = GetDistance(customer.City, ev.City);
                        }
                        if (!dicFiveClosest.ContainsKey(ev))
                        {
                            dicFiveClosest.Add(ev, dist);
                        }
                    }

                    var x = (from y in dicFiveClosest
                             orderby y.Value
                             select y).Take(5);

                    Console.WriteLine("5 closest events near customer are displayed as follows");
                    var radnom = new Random();
                    foreach (var item in x)
                    {
                        if (!dicEventWithPrice.ContainsKey(item.Key))
                        {
                            dicEventWithPrice.Add(item.Key, Convert.ToDecimal(radnom.NextDouble()));
                        }
                        SendCustomerNotifications(customer, item.Key);
                    }

                   var z = from evnt in dicEventWithPrice
                           orderby evnt.Value
                           select evnt;
                    Console.WriteLine("5 closest events near customer and then sorted by price are displayed as follows");
                    foreach (var item in z)
                    {
                        Console.WriteLine("event Price {0}", item.Value);
                        SendCustomerNotifications(customer, item.Key);
                    }

                }
                catch
                {
                    Console.WriteLine("Error while processing your request exiting with code {0}", HttpStatusCode.InternalServerError);
                }
            }
        }

    }


}
