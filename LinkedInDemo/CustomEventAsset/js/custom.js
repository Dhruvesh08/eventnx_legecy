/* ========== Select Dropdown js Strat ========== */ 
// $(".myval").select2({
//   width: "100%",
//   dropdownAutoWidth: true,
// });
/* ========== Select Dropdown js End ========== */

$(document).ready(function() {
  $(".js-select2").select2({
    width: "100%",
  });
  $(".js-select2-multi").select2();

  $(".large").select2({
    dropdownCssClass: "big-drop",
  });
});