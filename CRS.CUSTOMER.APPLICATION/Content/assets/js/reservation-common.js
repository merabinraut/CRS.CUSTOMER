function InitiateClubReservationFunction(ClubId, SelectedDate = "", SelectedHost = "") {
    EnableLoaderFunction();
    if (document.getElementById('club-bottom-tab-id')) {
        document.getElementById('club-bottom-tab-id').style.display = 'none';
    }
    $.ajax({
        type: 'GET',
        async: true,
        url: '/ReservationManagementV2/InitiateClubReservationProcess',
        dataType: 'json',
        data: {
            ClubId,
            SelectedDate: $('#date-id').val(),
            SelectedHost
        },
        success: function (data) {
            $("#stickey_id").css("display", "none")
            if (!data || data.Code !== 0) {
                toastr.info(data?.Message);
                DisableLoaderFunction();
                return false;
            }
            $('#clubreservationpartialview-id').html(data.PartialView);
            jQuery(function ($) {
                $.datepicker.regional['ja'] = {
                    closeText: '閉じる',
                    prevText: '&#x3c;前',
                    nextText: '次&#x3e;',
                    currentText: '今日',
                    monthNames: ['1月', '2月', '3月', '4月', '5月', '6月',
                        '7月', '8月', '9月', '10月', '11月', '12月'
                    ],
                    monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月',
                        '7月', '8月', '9月', '10月', '11月', '12月'
                    ],
                    dayNames: ['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日'],
                    dayNamesShort: ['月', '火', '水', '木', '金', '土', '日',],
                    dayNamesMin: ['月', '火', '水', '木', '金', '土', '日',],
                    weekHeader: '週',
                    dateFormat: 'yy/mm/dd',
                    firstDay: 0,
                    isRTL: false,
                    showMonthAfterYear: true,
                    yearSuffix: '年'
                };
                $.datepicker.setDefaults($.datepicker.regional['ja']);
                var currentDate = new Date();
                var maxDate = new Date(currentDate.getTime() + (14 * 24 * 60 * 60 * 1000)); // 15 days from today
                var dateList = JSON.parse(data.UnreservableDates);
                var formattedDates = [];
                dateList.forEach(function (dateString) {
                    if (dateString !== undefined) {
                        var dateParts = dateString.split('-');
                        if (dateParts.length === 3) {
                            var formattedDate = dateParts[0] + '-' + dateParts[1] + '-' + dateParts[2];
                            formattedDates.push(formattedDate);
                        }
                    }
                });
                $("#datepicker").datepicker({
                    minDate: currentDate,
                    maxDate: maxDate,
                    beforeShowDay: function (date) {
                        var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                        return [formattedDates.indexOf(string) == -1];
                    },
                    onSelect: function (dateText, inst) {
                        inst.inline = true; // Set datepicker to inline mode
                        $('#date-id').val(dateText.trim());
                    }
                });

                // Open the calendar by default
                $("#datepicker").datepicker("show");
                // Update selected date in datevalue element

            });
            initTimeFunction();
            initPeopleFunction();
            DisableLoaderFunction();
        },
        error: function (xhr, status, error) {
            toastr.info("Something went wrong. Please try again later.");
            DisableLoaderFunction();
            return false;
        }
    });
}


/////////////////////////////////////////////////////////////////////// Time JS ///////////////////////////////////////////////////////////////////////
function initTimeFunction() {
    var showTimeLists = document.querySelectorAll('.showTimeList');
    showTimeLists.forEach(function (showTimeList) {
        showTimeList.addEventListener('click', function (event) {
            var timeList = showTimeList.nextElementSibling;
            if (timeList.style.display === "none" || timeList.style.display === "") {
                timeList.style.display = "block";
            } else {
                timeList.style.display = "none";
            }
            event.stopPropagation(); // Prevents the click event from bubbling up to the document
        });
    });

    document.addEventListener('click', function (event) {
        // Loop through each showTimeList
        showTimeLists.forEach(function (showTimeList) {
            var timeList = showTimeList.nextElementSibling;
            // Check if the click target is not the showTimeList, its timeList, or their descendants and close the timeList if it's open
            if (!showTimeList.contains(event.target) && !timeList.contains(event.target) && timeList.style
                .display === "block") {
                timeList.style.display = "none";
            }
        });
    });

    document.querySelectorAll('.timeList').forEach(item => {
        item.addEventListener('click', event => {
            if (event.currentTarget.classList.contains('disabled')) {
                return;
            }
            document.querySelectorAll('.timeList').forEach(item => {
                item.classList.remove('active');
            });
            event.currentTarget.classList.add('active');

            // Store the value if timeList has active class
            if (event.currentTarget.classList.contains('active')) {
                const timeValue = event.currentTarget.querySelector('.timeValue').textContent;
                var selectedTimeDiv = document.getElementById("selected-time-id");
                selectedTimeDiv.innerText = timeValue.trim();
                $('#time-id').val(timeValue.trim());
            }
        });
    });
}
/////////////////////////////////////////////////////////////////////// Time JS ///////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////// People JS ///////////////////////////////////////////////////////////////////////
function initPeopleFunction() {
    var showPeopleLists = document.querySelectorAll('.showPeopleList');
    showPeopleLists.forEach(function (showPeopleList) {
        showPeopleList.addEventListener('click', function (event) {
            var timeList = showPeopleList.nextElementSibling;
            if (timeList.style.display === "none" || timeList.style.display === "") {
                timeList.style.display = "block";
            } else {
                timeList.style.display = "none";
            }
            event.stopPropagation(); // Prevents the click event from bubbling up to the document
        });
    });

    document.addEventListener('click', function (event) {
        // Loop through each showTimeList
        showPeopleLists.forEach(function (showPeopleList) {
            var timeList = showPeopleList.nextElementSibling;
            // Check if the click target is not the showTimeList, its timeList, or their descendants and close the timeList if it's open
            if (!showPeopleList.contains(event.target) && !timeList.contains(event.target) && timeList.style
                .display === "block") {
                timeList.style.display = "none";
            }
        });
    });

    document.querySelectorAll('.peopleList').forEach(item => {
        item.addEventListener('click', event => {
            document.querySelectorAll('.peopleList').forEach(item => {
                item.classList.remove('active');
            });
            event.currentTarget.classList.add('active');
            // Store the value if timeList has active class
            if (event.currentTarget.classList.contains('active')) {
                const peopleValue = event.currentTarget.querySelector('.peopleValue').textContent;
                //console.log('Time value:', peopleValue.trim());
                var selectedPeopleDiv = document.getElementById("selected-noofpeople-id");
                selectedPeopleDiv.innerText = peopleValue.trim();
                $('#noofpeople-id').val(peopleValue.trim());

            }
            if (parseInt($("#selected-noofpeople-id").text()) > 3) {
                $("#waitingMessageWrapper").css("display", "block");
            }
            else {
                $("#waitingMessageWrapper").css("display", "none");
            }
        });
    });
}
/////////////////////////////////////////////////////////////////////// People JS ///////////////////////////////////////////////////////////////////////

function CloseInitiatedClubReservationFunction() {
    $("#stickey_id").css("display", "")
    var element = document.getElementById('drawer-date');
    element.classList.add('translate-y-full');
    var removeElement = document.getElementById("ui-datepicker-div")
    removeElement.style.setProperty("display", "none", "important");
    if (document.getElementById('club-bottom-tab-id')) {
        document.getElementById('club-bottom-tab-id').style.display = '';
    }
}

function SubmitClubReservationFunction() {
    var form = document.getElementById('club-reservation-id');
    var requiredFields = form.querySelectorAll('[required]');
    for (var i = 0; i < requiredFields.length; i++) {
        if (!requiredFields[i].value) {
            /*toastr.info('Please fill out all required fields.');*/
            toastr.info('日付が必要です');
            return; // Prevent form submission
        }
    }
    // If all required fields are filled, submit the form
    form.submit();
}

/*function EnableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'block';
    
    document.body.classList.add('no-scroll-loader');
}
function DisableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'none';
    document.body.classList.remove('no-scroll-loader');
}
*/
function preventDefault(event) {
    event.preventDefault();
}
function EnableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'flex';
    document.body.addEventListener('touchmove', preventDefault, { passive: false });
    document.body.classList.add('no-scroll-loader');
}
function DisableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'none';
    document.body.removeEventListener('touchmove', preventDefault, { passive: false });
    document.body.classList.remove('no-scroll-loader');
}
