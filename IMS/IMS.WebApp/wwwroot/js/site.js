// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Add multiselect select2
$(document).ready(function () {

	$('.select2').select2({
		placeholder: 'Select an option',
		closeOnSelect: false,
	});

	// For only one multi select
	$('.select2').on("change", function (e) {
		if ($(this).val() == null) {
			$('.select2_validation').show();
		} else {
			$('.select2_validation').hide();
		}
	});

	// For single

	// JOB - Create
	$('#multiSkill').on("change", function (e) {
		if ($(this).val() == null) {
			$('#multiSkill_validation').show();
		} else {
			$('#multiSkill_validation').hide();
		}
	});
	$('#multiLevel').on("change", function (e) {
		if ($(this).val() == null) {
			$('#multiLevel_validation').show();
		} else {
			$('#multiLevel_validation').hide();
		}
	});
	$('#multiBenefit').on("change", function (e) {
		if ($(this).val() == null) {
			$('#multiBenefit_validation').show();
		} else {
			$('#multiBenefit_validation').hide();
		}
	});
});