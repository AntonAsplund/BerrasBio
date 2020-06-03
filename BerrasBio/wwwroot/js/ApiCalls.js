let searchButton = document.getElementById("search");
searchButton.addEventListener("click", Search)

async function Search()
{
    document.getElementById("movieInfo").classList.add("d-none");

    let searchTerm = document.getElementById("searchTerm").value;
    let realSearchTerm = searchTerm.replace(/ /g, "+");

    let url = `https://www.omdbapi.com/?t=${realSearchTerm}&plot=full&apikey=a08ab6da`;

    let response = await fetch(url);
    let json = await response.json();

    if (json.Error)
    {
        document.getElementById("error").innerHTML = json.Error;
    }

    else
    {
        if (response.ok)
        {
            document.getElementById("error").innerHTML = "";

            document.getElementById("movieInfo").classList.remove("d-none");
            let minutes = json.Runtime.split(" ");
            let pg = json.Rated.split("-");

            document.getElementById("title").value = json.Title;
            document.getElementById("lenght").value = minutes[0];
            document.getElementById("category").value = json.Genre;
            document.getElementById("year").value = json.Year;
            document.getElementById("plot").value = json.Plot;
            document.getElementById("director").value = json.Director;
            document.getElementById("actors").value = json.Actors;
            document.getElementById("pg").value = pg[1];
            document.getElementById("url").value = json.Poster;

            console.log(json.Director);
            console.log(json.Actors);
        }
    }
}