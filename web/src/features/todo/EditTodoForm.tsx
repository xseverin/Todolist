import React, { useState, useEffect } from 'react';
import { UpdateTodoApi } from '../todo/todoAPI.ts';

interface Todo {
    id: string;
    name: string;
    done: boolean;
}

interface EditTodoFormProps {
    todo: Todo;
    onSave: () => void;
    onCancel: () => void;
}

const EditTodoForm: React.FC<EditTodoFormProps> = ({ todo }) => {
    const [name, setName] = useState(todo.name);

    useEffect(() => {
        setName(todo.name);
    }, [todo]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await UpdateTodoApi(todo.id, name);
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <input
                    type="text"
                    id="name"
                    value={name}
                    className={todo.done ? 'line-through hover:no-underline opacity-50' : ''}
                    style={{backgroundColor: 'transparent'}}
                    onChange={(e) => setName(e.target.value)}
                />
            </div>
        </form>
    );
};

export default EditTodoForm;