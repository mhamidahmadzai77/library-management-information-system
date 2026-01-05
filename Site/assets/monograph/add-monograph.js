

function continueButtonEvent() {

    var tab3 = document.getElementById('tab3');
    if (tab3.classList.contains('active')) {
        
        document.getElementById('studentRegistrationYearParagraph').innerHTML = document.getElementById('studentRegistrationYear').value;
        document.getElementById('studentGraduationYearParagraph').innerHTML = document.getElementById('studentGraduationYear').value;
        document.getElementById('defenceDateParagraph').innerHTML = document.getElementById('defenceDate').value;
        document.getElementById('markParagraph').innerHTML = document.getElementById('mark').value;
        document.getElementById('graduationPeriodParagraph').innerHTML = document.getElementById('graduationPeriod').value;

        document.getElementById('bookNameTD').innerHTML = document.getElementById('name').value;
        document.getElementById('publicationQuantityTD').innerHTML = document.getElementById('publicationQuantity').value;
        document.getElementById('publicationPagesTD').innerHTML = document.getElementById('publicationPages').value;
        document.getElementById('publicationCDTD').innerHTML = document.getElementById('CDQuantity').value;
        document.getElementById('registrationDateTypeTD').innerHTML = document.getElementById('registrationDateType').value;
        document.getElementById('registrationDateTD').innerHTML = document.getElementById('registrationDate').value;
        document.getElementById('publicationDateTypeTD').innerHTML = document.getElementById('publicationDateType').value;
        document.getElementById('publicationYearTD').innerHTML = document.getElementById('publicationYear').value;
        document.getElementById('publicationMonthTD').innerHTML = document.getElementById('publicationMonth').value;
        document.getElementById('publicationDayTD').innerHTML = document.getElementById('publicationDay').value;
        document.getElementById('cupboardNoTD').innerHTML = document.getElementById('cupboardNo').value;
        document.getElementById('cellNoTD').innerHTML = document.getElementById('cellNo').value;
    }

}
