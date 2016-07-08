$(document).ready(function () {
    $('#tabs div').hide(); // Hide all divs
    $('#tabs div:first').show(); // Show the first div
    $('#tabs ul li:first').addClass('active'); // Set the class of the first link to active
    $('#tabs ul li a').click(function () { //When any link is clicked
        $('#tabs ul li').removeClass('active'); // Remove active class from all links
        $(this).parent().addClass('active'); //Set clicked link class to active
        var currentTab = $(this).attr('href'); // Set variable currentTab to value of href attribute of clicked link
        $('#tabs div').hide(); // Hide all divs
        $(currentTab).show(); // Show div with id equal to variable currentTab
        $('#slider1').bxSlider();
        return false;
    });
});
    