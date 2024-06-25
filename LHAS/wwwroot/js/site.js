/*Alert button*/
$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

/*DataTable*/
$(document).ready(function () {
    // Find the index of the column with the empty header
    let emptyHeaderIndex = [];
    $('#tables thead th').each(function (index) {
        if ($(this).text().trim() === '') {
            emptyHeaderIndex.push(index);
        }
    });

    // Initialize DataTable with columnDefs
    $('#tables').DataTable({
        "columnDefs": [
            {
                "targets": "_all", // Target all columns
                "className": "text-center" // Apply 'text-center' class to center align content
            },
            {
                "targets": emptyHeaderIndex, // Target columns with empty header
                "searchable": false // Make these columns not searchable
            }
        ]
    });
});

/*DatePicker*/

document.addEventListener("DOMContentLoaded", function() {
    var today = new Date();
    var todayFormatted = today.getFullYear() + "-" + ('0' + (today.getMonth() + 1)).slice(-2) + "-" + ('0' + today.getDate()).slice(-2);
    var dateTimePicker = flatpickr("#datetimepicker", {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
        time_24hr: true,
        minTime: "09:00",
        maxTime: "18:00",
        minDate: todayFormatted,
        inline: true, // I am using this for fix the callendar
        onChange: function(selectedDates, dateStr, instance) {
            document.querySelector("#datetimepicker").value = dateStr; // updating field with the selected date
        },
        onReady: function(selectedDates, dateStr, instance) {
            document.querySelector("#datetimepicker").setAttribute("required", "required"); 
        }
    });

    document.querySelector("form").addEventListener("submit", function(event) {
        if (!dateTimePicker.selectedDates.length) {
            event.preventDefault();
            event.stopPropagation();
            alert("Please fill out date field.");
        }
        this.classList.add('was-validated');
    });
});