import React, { useState, useEffect, useRef } from 'react';
import { UpdateTodoApi } from '../todo/todoAPI.ts';

interface Todo {
    Id: string;
    Name: string;
    Done: boolean;
}

interface EditTodoFormProps {
    todo: Todo;
    disableEdit?: boolean; // Prop for disabling edit
}

const EditTodoForm: React.FC<EditTodoFormProps> = ({ todo, disableEdit = false }) => {
    const [Name, setName] = useState(todo.Name);
    const [error, setError] = useState<string | null>(null);
    const textareaRef = useRef<HTMLTextAreaElement | null>(null);

    useEffect(() => {
        setName(todo.Name);
    }, [todo]);

    useEffect(() => {
        adjustTextareaHeight();
    }, [Name]);

    const adjustTextareaHeight = () => {
        if (textareaRef.current) {
            textareaRef.current.style.height = 'auto';
            textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`;
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        if (!disableEdit) {
            setName(e.target.value);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (disableEdit) return;

        try {
            await UpdateTodoApi(todo.Id, Name);
        } catch (err) {
            setError('Failed to update todo. Please try again.');
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (!disableEdit && e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSubmit(e);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="w-full">
            <span className="relative">
                <textarea
                    id="name"
                    value={Name}
                    ref={textareaRef}
                    className={`focus:outline-none focus:ring-2 focus:ring-blue-300 w-full bg-transparent duration-300 ${todo.Done ? 'line-through opacity-50' : ''}`}
                    onChange={handleChange}
                    onKeyDown={handleKeyDown}
                    rows={1}
                    disabled={disableEdit} // Disable editing
                    style={{ resize: 'none', overflow: 'hidden', lineHeight: '1.5em', cursor: disableEdit ? 'default' : 'text' }} // Pointer changes based on state
                />
                {error && <p className="text-red-500 text-sm mt-2">{error}</p>}
            </span>
        </form>
    );
};

export default EditTodoForm;
