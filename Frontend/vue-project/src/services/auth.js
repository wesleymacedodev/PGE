import http from "./http.js"

async function validate() {
    try {
        await http.get("/Login/Validate", {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("token")}`
            }
        })
        return true
    } catch (error) {
        return false
    }

}

export default validate

