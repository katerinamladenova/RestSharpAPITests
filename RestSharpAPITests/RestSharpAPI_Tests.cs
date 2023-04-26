using RestSharp;
using System.Net;
using System.Text.Json;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseURL = "https://contactbookkatia.katerinamladeno.repl.co/api";

        [SetUp]
        public void SetUp()
        {
            this.client = new RestClient(baseURL);
        }

        [Test]
        public void Test_ListAllContacts_VerifyFirstContactNames()
        {
            //Arrange
            var request = new RestRequest("/contacts", Method.Get);

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));

        }

        [Test]
        public void Test_FindContact_VerifyContactNames()
        {
            //Arrange
            var request = new RestRequest("/contacts/search/albert", Method.Get);

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));

        }

        [Test]
        public void Test_FindContact_EmptyResults()
        {
            //Arrange
            var request = new RestRequest($"/contacts/search/missing{DateTime.Now.Ticks}", Method.Get);

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.EqualTo("[]"));
            //var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            //Assert.That(contacts, Is.Empty);

        }

        [Test]
        public void Test_CreateContactInvalidData_ReturnsErrorMsg()
        {
            //Arrange
            var request = new RestRequest("/contacts", Method.Post);
            var requestBody = new Contact
            {
                lastName = "Doe",
                email = "johnTest@gmail.com",
                phone = "+1 800 200 3110",
                comments = "Ex friend"
            };
            request.AddBody(requestBody);

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"First name cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateContactValidData_VerifyCreatedCorrect()
        {
            //Arrange
            var request = new RestRequest("/contacts", Method.Post);
            var requestBody = new Contact
            {
                firstName = "Drenka" + DateTime.Now.Ticks,
                lastName = "Filipova" + DateTime.Now.Ticks,
                email = "drenka1988@abv.bg",
                phone = "+1 333 200 3110",
                comments = "Best friend"
            };
            request.AddBody(requestBody);

            //Act
            var response = this.client.Execute(request);
            var contactObject = JsonSerializer.Deserialize<contactObject>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(contactObject.msg, Is.EqualTo("Contact added."));
            Assert.That(contactObject.contact.id, Is.GreaterThan(0));
            Assert.That(contactObject.contact.firstName, Is.EqualTo(requestBody.firstName));
            Assert.That(contactObject.contact.lastName, Is.EqualTo(requestBody.lastName));
            Assert.That(contactObject.contact.email, Is.EqualTo(requestBody.email));
            Assert.That(contactObject.contact.phone, Is.EqualTo(requestBody.phone));
            Assert.That(contactObject.contact.dateCreated.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(contactObject.contact.comments, Is.EqualTo(requestBody.comments));

        }

    }
}