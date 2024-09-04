// src/features/todo/todoSlice.ts
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { AddTodoApi, DeleteAllDoneApi, FetchTodosApi } from './todoAPI';

// Define the initial state for todos
interface Todo {
    id: string;
    name: string;
    done: boolean;
    parentId?: string;
    children?: Todo[];
}

interface TodoState {
    todos: Todo[];
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: TodoState = {
    todos: [],
    status: 'idle',
    error: null,
};

// Async thunk for fetching todos
export const fetchTodos = createAsyncThunk('todos/fetchTodos', async () => {
    const response = await FetchTodosApi();
    return response;
});

// Async thunk for adding a new todo
export const addTodo = createAsyncThunk(
    'todos/addTodo',
    async (newTodo: { name: string; parentId?: string }) => {
        await AddTodoApi(newTodo.name, newTodo.parentId);
        return { name: newTodo.name, parentId: newTodo.parentId };
    }
);

// Async thunk for deleting all completed todos
export const deleteAllDone = createAsyncThunk('todos/deleteAllDone', async () => {
    await DeleteAllDoneApi();
    return;
});

const todoSlice = createSlice({
    name: 'todos',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchTodos.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchTodos.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.todos = action.payload;
            })
            .addCase(fetchTodos.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message || 'Failed to fetch todos';
            });
    },
});

export default todoSlice.reducer;
