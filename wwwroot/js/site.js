/*Alert button*/
$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

document.addEventListener("DOMContentLoaded", function() {
    var dateTimePicker = flatpickr("#datetimepicker", {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
        time_24hr: true,
        minTime: "09:00",
        maxTime: "18:00",
        onReady: function(selectedDates, dateStr, instance) {
            instance.input.setAttribute("required", "required");
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