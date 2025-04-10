window.onload = () => {
    document.getElementById("formLogin").addEventListener("submit", async (e) => {
        e.preventDefault();
        const userName = document.getElementById("userName").value;
        const password = document.getElementById("password").value;

        const response = await fetch("http://localhost:5017/api/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ userName, password })
        });

        if (!response.ok) {
            document.getElementById("error").innerText = "שגיאה בהיתחברות";
            return;
        }

        const data = await response.json();

        if (!data.token) {
            document.getElementById("error").innerText = "שגיאה: חסר טוקן";
            return;
        }

        localStorage.setItem("token", data.token);

        // ✨ שליפת פרטי המשתמש
        const userResponse = await fetch("http://localhost:5017/user/me", {
            headers: {
                "Authorization": `Bearer ${data.token}`
            }
        });

        if (!userResponse.ok) {
            document.getElementById("error").innerText = "שגיאה בקבלת פרטי משתמש";
            return;
        }

        const user = await userResponse.json();
        localStorage.setItem("user", JSON.stringify(user));

        window.location.href = "profile.html";
    });
};

const logout = () => {
    localStorage.clear();
    window.location.href = "index.html";
};
