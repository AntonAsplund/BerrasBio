// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let checkboxValues = [];
let numTickets = 0;
const callback = event => {
    let seatCounter = document.getElementById("seat-count");
    var dirty_bit = document.getElementById('page_is_dirty');
    if (dirty_bit.value == '1') window.location.reload();
    function mark_page_dirty() {
        dirty_bit.value = '1';
    }
    for (var i = 1; i < 51; i++) {

        let seat_container = document.getElementById(i);
        if (seat_container != null) {
            const newLocal = `#${i} .check-seat`;
            
            console.log(newLocal);
            let checkbox = seat_container.childNodes[1];
            
            if (checkbox != null) {
                if (!checkbox.checked) {
                    seat_container.addEventListener('click', () => {
                            if (checkbox.checked) {
                                seat_container.style.backgroundColor = "green";
                                numTickets--;
                                checkbox.checked = false;
                            } else {
                                if (numTickets < 14) {
                                    seat_container.style.backgroundColor = "blue";
                                    checkbox.checked = true;
                                    numTickets++;
                                } else {
                                    checkbox.checked = false;

                                }
                            }
                        checkbox.dispatchEvent(new Event('change'));
                        seatCounter.innerText = numTickets;
                    });
                } else {
                    checkbox.style.display = "none";
                    seat_container.style.backgroundColor = "red";
                }
            }
        }

    }
    mark_page_dirty();
}


window.addEventListener('load', callback);