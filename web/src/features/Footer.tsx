import { Footer } from "antd/es/layout/layout";

export const AppFooter = () => {
    return (
        <Footer style={{
            textAlign: "center",
            backgroundColor: "rgb(0, 21, 41)",
            color: "white",
            height: "5px",  // Adjust height here
            lineHeight: "5px", // Ensure the text is vertically centered
            fontSize: "15px"   // Adjust font size here
        }}>
            Todolist ©2024 Created by Yevhenii Severin
        </Footer>
    );
};
