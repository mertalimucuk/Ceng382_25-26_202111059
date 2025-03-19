document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("classForm");
  const tableBody = document.querySelector("#classTable tbody");

  // AI PROMPT : Form submit edildiğinde sayfa yenilenmeden tabloya veri nasıl eklenir?
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
    // AI PROMPT : Yeni tablo satırını dinamik olarak nasıl ekleyebilirim?
    const newRow = document.createElement("tr");
    newRow.innerHTML = `
            <td>${className}</td>
            <td>${numPeople}</td>
            <td>${description}</td>
        `;

    tableBody.appendChild(newRow);
    form.reset();
  });

  // AI PROMPT: Tabloya tıklanınca tüm verileri JSON formatında nasıl yazdırabilirim? (ROW CLİCK EVENT 1.ÖZELLİK KULLANDIĞIM HOCAM ) (Table Click Event 2. ÖZELLİK KULLANDIĞIM HOCAM)

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

  //AI PROMPT: Bir satıra tıklanınca stil değişikliği (highlight) nasıl eklenir?
  tableBody.addEventListener("click", function (event) {
    if (event.target.tagName === "TD") {
      let row = event.target.parentElement;
      row.classList.toggle("highlight");
    }
  });

  tableBody.addEventListener("click", function (event) {
    if (event.target.tagName === "TD") {
      let row = event.target.parentElement;
      row.style.backgroundColor =
        row.style.backgroundColor === "yellow" ? "" : "yellow";
    }
  });

  //Mouseover Event kullandığım 3. özellik hocam
  tableBody.addEventListener("mouseover", function (event) {
    if (event.target.tagName === "TD") {
      event.target.parentElement.style.backgroundColor = "#f0f0f0";
    }
  });

  //Mouseout Event kullandığım 4.özellik hocam
  tableBody.addEventListener("mouseout", function (event) {
    if (event.target.tagName === "TD") {
      event.target.parentElement.style.backgroundColor = "";
    }
  });

  //SATIRA CİFT TIKLANINCA SİLİNMESİNİ SAGLAYAN KOD Double-click Event kullandığım 5.özellik hocam
  tableBody.addEventListener("dblclick", function (event) {
    if (event.target.tagName === "TD") {
      event.target.parentElement.remove();
    }
  });

  //AI PROMPT: Input alanına tıklanınca border rengini değiştirme işlemi nasıl yapılır? ( Input Focus Event 6.öZELLİK HOCAM VE  Input Blur Event 7. Özellik )
  document.querySelectorAll("input").forEach((input) => {
    input.addEventListener("focus", function () {
      this.style.border = "2px solid black";
    });

    input.addEventListener("blur", function () {
      this.style.border = "";
      if (this.value.trim() === "") {
        alert(`${this.placeholder} boş bırakılamaz!`);
      }
    });
  });

  //AI PROMPT: Input içine canlı doğrulama (real-time validation) nasıl eklenir? Keyup/Keydown Event  kullandığım 8.özellik hocam
  document.getElementById("numPeople").addEventListener("keyup", function () {
    if (isNaN(this.value)) {
      this.style.border = "2px solid red";
    } else {
      this.style.border = "2px solid green";
    }
  });
});
