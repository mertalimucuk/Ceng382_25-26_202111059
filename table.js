document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("classForm");
  const tableBody = document.querySelector("#classTable tbody");

  // ✅ Form Submit Event - Sayfa yenilenmeden tabloya veri ekleme
  form.addEventListener("submit", function (event) {
    event.preventDefault();

    const className = document.getElementById("className").value;
    const numPeople = document.getElementById("numPeople").value;
    const description = document.getElementById("description").value;

    if (
      className.trim() === "" ||
      numPeople.trim() === "" ||
      description.trim() === ""
    ) {
      alert("Tüm alanları doldurun!");
      return;
    }

    const newRow = document.createElement("tr");
    newRow.innerHTML = `
            <td>${className}</td>
            <td>${numPeople}</td>
            <td>${description}</td>
        `;

    tableBody.appendChild(newRow);
    form.reset();
  });

  // ✅ Tabloya tıklanınca tüm verileri console’a yazdırma (Table Click Event)
  document.getElementById("classTable").addEventListener("click", function () {
    let rows = document.querySelectorAll("#classTable tbody tr");
    let classList = [];

    rows.forEach((row) => {
      let rowData = {
        className: row.cells[0].textContent,
        numPeople: row.cells[1].textContent,
        description: row.cells[2].textContent,
      };
      classList.push(rowData);
    });

    console.log("Current Table Data:", classList);
  });

  // ✅ Satıra tıklanınca highlight ekleme (Row Click Event)
  tableBody.addEventListener("click", function (event) {
    if (event.target.tagName === "TD") {
      let row = event.target.parentElement;
      row.classList.toggle("highlight");
    }
  });

  // ✅ Satır üzerine gelince geçici renk değişimi (Mouseover / Mouseout Events)
  tableBody.addEventListener("mouseover", function (event) {
    if (event.target.tagName === "TD") {
      event.target.parentElement.style.backgroundColor = "#f0f0f0";
    }
  });

  tableBody.addEventListener("mouseout", function (event) {
    if (event.target.tagName === "TD") {
      event.target.parentElement.style.backgroundColor = "";
    }
  });

  // 🔥 EKSTRA: Satıra çift tıklanınca satır silinsin (Double-click Event)
  tableBody.addEventListener("dblclick", function (event) {
    if (event.target.tagName === "TD") {
      event.target.parentElement.remove();
    }
  });

  // 🔥 Input Focus & Blur Events - Input'a tıklanınca stil değişimi
  document.querySelectorAll("input").forEach((input) => {
    input.addEventListener("focus", function () {
      this.style.border = "2px solid blue";
    });

    input.addEventListener("blur", function () {
      this.style.border = "";
      if (this.value.trim() === "") {
        alert(`${this.placeholder} boş bırakılamaz!`);
      }
    });
  });

  // 🔥 Keyup Event - Canlı doğrulama
  document.getElementById("numPeople").addEventListener("keyup", function () {
    if (isNaN(this.value)) {
      this.style.border = "2px solid red";
    } else {
      this.style.border = "2px solid green";
    }
  });
});
