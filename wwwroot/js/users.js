window.onload = async () => {
    const backButton = document.createElement("button");
    backButton.innerText = "🔙 חזרה לספרים";
    backButton.onclick = () => window.location.href = "profile.html";
    document.body.insertBefore(backButton, document.getElementById("bookList"));

    const token = localStorage.getItem("token");
    if (!token) return window.location.href = "index.html";

    let user;
    try {
        user = JSON.parse(localStorage.getItem("user"));
    } catch {
        return window.location.href = "index.html";
    }

    if (!user || user.role !== "Admin") return window.location.href = "profile.html";

    await loadUsers();

    document.getElementById("addUserForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        const name = document.getElementById("newUserName").value;
        const password = document.getElementById("newPassword").value;
        const role = document.getElementById("newRole").value;

        const res = await fetch("http://localhost:5017/user", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({ name, password, role })
        });

        const message = document.getElementById("message");
        if (res.ok) {
            message.innerText = "✅ משתמש נוסף בהצלחה!";
            document.getElementById("addUserForm").reset();
            document.getElementById("addUserForm").style.display = "none";
            await loadUsers();
            setTimeout(() => message.innerText = "", 3000);
        } else {
            message.innerText = "❌ שגיאה בהוספה.";
            setTimeout(() => message.innerText = "", 3000);
        }
    });

    document.getElementById("editUserForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        const id = +document.getElementById("editUserId").value;
        const name = document.getElementById("editUserName").value;
        const password = document.getElementById("editPassword").value;
        const role = document.getElementById("editRole").value;

        const body = { id, name, role };
        if (password) body.password = password;

        const res = await fetch(`http://localhost:5017/user/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(body)
        });

        const message = document.getElementById("message");
        if (res.ok) {
            message.innerText = "✅ המשתמש עודכן בהצלחה!";
            cancelEdit();
            await loadUsers();
            setTimeout(() => message.innerText = "", 3000);
        } else {
            message.innerText = "❌ שגיאה בעדכון המשתמש";
            setTimeout(() => message.innerText = "", 3000);
        }
    });

    document.getElementById("cancelEditBtn").addEventListener("click", cancelEdit);

    // 🔄 פתיחה/סגירה של טופס הוספת משתמש
    document.getElementById("toggleAddUserBtn").addEventListener("click", () => {
        const form = document.getElementById("addUserForm");
        form.style.display = form.style.display === "none" ? "block" : "none";
    });
};


 const loadUsers = async() =>{
    const token = localStorage.getItem("token");
    const res = await fetch("http://localhost:5017/user", {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (!res.ok) return;

    const users = await res.json();
    const table = document.getElementById("usersTable");
    table.innerHTML = "";

    users.forEach(user => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${user.id}</td>
            <td>${user.name}</td>
            <td>${user.password}</td>
            <td>${user.role}</td>
            <td>
                <button onclick="deleteUser(${user.id})">🗑️</button>
                <button onclick="editUser(${user.id}, '${user.name}', '${user.role}')">✏️</button>
            </td>
        `;
        table.appendChild(tr);
    });
}

window.editUser = (id, name, role) => {
    document.getElementById("editUserId").value = id;
    document.getElementById("editUserName").value = name;
    document.getElementById("editRole").value = role;
    document.getElementById("editPassword").value = "";
    document.getElementById("editUserFormContainer").style.display = "block";
};

window.deleteUser = async (id) => {
    const token = localStorage.getItem("token");

    const res = await fetch(`http://localhost:5017/user/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    });

    const message = document.getElementById("message");
    if (res.ok) {
        message.innerText = "✅ המשתמש נמחק";
        await loadUsers();
    } else {
        message.innerText = "❌ שגיאה במחיקה";
    }

    setTimeout(() => message.innerText = "", 3000);
};

const cancelEdit = () => {
    document.getElementById("editUserFormContainer").style.display = "none";
}
