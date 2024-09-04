import axios from "axios";
const BASE_URL = import.meta.env.VITE_BASE_URL;
export const AddTodoApi = async (name: string, parentId?: string) => {
    const requestBody: any = { name };

    if (parentId) {
        requestBody.parentId = parentId;
    }

    try {
        const response = await axios.post(`${BASE_URL}/Todo/add`, requestBody);
        return response.data;
    } catch (error) {
        console.log(error);
        return null;
    }
};

export const UpdateTodoApi = async ( id: string, name : string ) => {
    const response = await axios.post(`${BASE_URL}/Todo/Update/${id}`,
        {
            name
        }).catch((ex)=>{
        console.log(ex);
    });
    return response?.data;
};

export const FetchTodosApi = async (): Promise<any> => {
    try {
        const response = await axios.get(`${BASE_URL}/Todo/GetTodos`);
        return response.data;
    } catch (error) {
        console.error('Error fetching todos:', error);
        throw error;
    }
};

export const DeleteAllDoneApi = async (): Promise<void> => {
    try {
        await axios.delete(`${BASE_URL}/Todo/DeleteAllDone`);
    } catch (error) {
        console.error('Error deleting all done todos:', error);
        throw error;
    }
};

export const deleteTodoApi = async (id : string): Promise<void> => {
    try {
        await axios.delete(`${BASE_URL}/Todo/delete/${id}`);
    } catch (error) {
        console.error('Error deleting all done todos:', error);
        throw error;
    }
};

export const toggleTodoApi = async ( id : string, checked : boolean ) => {
    try {
        return await axios.post(`${BASE_URL}/Todo/${checked ? 'MakeUndone' : 'MakeDone'}/${id}`);
    } catch (error) {
        console.error('Error updating todo status:', error);
    }
};

export const shareTodoApi = async (email: string, todoId: string) => {
    try {
        const response = await axios.post(`${BASE_URL}/Todo/share`, {
            email,
            todoId
        });
        return response.data;
    } catch (error) {
        console.error('Error sharing todo:', error);
        throw error;
    }
};

export const fetchSharedApi = async (): Promise<any> => {
    try {
        const response = await axios.get(`${BASE_URL}/Todo/GetSharedTodos`);
        return response.data;
    } catch (error) {
        console.error('Error fetching todos:', error);
        throw error;
    }
};