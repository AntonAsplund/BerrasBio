var startTime = document.getElementById("startTime");
var availableSeats = document.getElementById("availableSeats");

startTime.addEventListener("click", sortByTime);
availableSeats.addEventListener("click", sortBySeats);

function sortByTime() {
    var table = document.querySelector('#movies');
    var rows, switching, i, x, y, shouldSwitch, switchcount = 0, dir, order;

    if (table.classList.contains('asc')) {
        order = 'desc';
        table.classList.remove('asc');
        table.classList.add('desc');
    }

    else if (table.classList.contains('desc')) {
        order = 'asc';
        table.classList.remove('desc');
        table.classList.add('asc');
    }

    else {
        table.classList.add('asc');
        order = 'asc';
    }

    dir = order;

    switching = true;

    while (switching) {
        switching = false;
        rows = table.rows;

        // Loopa igenom alla rader uton den första(där är table headers)
        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;

            // Jämför två element
            x = rows[i].getElementsByTagName("TD")[0].innerText;
            y = rows[i + 1].getElementsByTagName("TD")[0].innerText;

            let xtime = x.split(' ');
            let ytime = y.split(' ');

            // Kollar om dom ska byta plats
            if (dir == "asc")
            {
                if (xtime[1] > ytime[1]) {
                    shouldSwitch = true;
                    break;
                }
            }

            else if (dir == "desc")
            {
                if (xtime[1] < ytime[1]) {
                    shouldSwitch = true;
                    break;
                }
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;

            switchcount++;
        }
    }

}

function sortBySeats() {
    var table = document.querySelector('#movies');
    var rows, switching, i, x, y, shouldSwitch, switchcount = 0, dir, order;

    if (table.classList.contains('asc')) {
        order = 'desc';
        table.classList.remove('asc');
        table.classList.add('desc');
    }

    else if (table.classList.contains('desc')) {
        order = 'asc';
        table.classList.remove('desc');
        table.classList.add('asc');
    }

    else {
        table.classList.add('asc');
        order = 'asc';
    }

    dir = order;
    switching = true;

    while (switching) {
        switching = false;
        rows = table.rows;

        // Loopa igenom alla rader uton den första(där är table headers)
        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;

            // Jämför två element
            x = rows[i].getElementsByTagName("TD")[2];
            y = rows[i + 1].getElementsByTagName("TD")[2];

            if (dir == "asc") {
                if (Number(x.innerHTML) > Number(y.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (Number(x.innerHTML) < Number(y.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;

            switchcount++;
        }
    }
}