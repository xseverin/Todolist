import React, { useState, useEffect, useRef, FormEvent } from 'react';
import { AddTodoApi, UpdateTodoApi } from '../todo/todoAPI.ts';
import { Todo } from './Todo.ts';

interface TodoFormProps {
    todo?: Todo; // Optional prop for edit mode
    parentId?: string;
    shouldHideForm?: boolean; // Prop to control form visibility
    disableEdit?: boolean; // Prop for disabling edit
    fetchTodos: () => void; // Function to fetch todos
    onVisibilityChange?: (isVisible: boolean) => void;
    className?: string; // Add className prop
    showPlaceholder?: boolean; // Prop for showing placeholder
    onHeightChange?: (newHeight: number) => void; // New prop for height change callback
}

const TodoForm: React.FC<TodoFormProps> = ({
                                               todo,
                                               parentId,
                                               shouldHideForm = false,
                                               disableEdit = false,
                                               fetchTodos,
                                               onVisibilityChange,
                                               className,
                                               showPlaceholder = false, // Default to false if not provided
                                               onHeightChange // New prop
                                           }) => {
    const [name, setName] = useState(todo ? todo.Name : '');
    const [show, setShow] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const textareaRef = useRef<HTMLTextAreaElement | null>(null);

    useEffect(() => {
        if (todo) {
            setName(todo.Name);
        }
    }, [todo]);

    useEffect(() => {
        adjustTextareaHeight();
        if (onHeightChange && textareaRef.current) {
            onHeightChange(textareaRef.current.offsetHeight);
        }
    }, [name]);

    const adjustTextareaHeight = () => {
        if (textareaRef.current) {
            textareaRef.current.style.height = 'auto';
            textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`;
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        if (!disableEdit || todo) {
            setName(e.target.value);
        }
    };

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        if (!name.trim()) {
            return;
        }

        try {
            if (todo) {
                await UpdateTodoApi(todo.Id, name);
            } else {
                await AddTodoApi(name, parentId);
            }
            setName('');
            setError(null);

            if (shouldHideForm && onVisibilityChange) {
                onVisibilityChange(false);
            }

            fetchTodos(); // Refresh the todos list after adding or updating
        } catch (err) {
            setError('Failed to process todo. Please try again.');
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (e.key === 'Enter' && !e.shiftKey) { // Allow multi-line input with Shift+Enter
            e.preventDefault();
            handleSubmit(e);
        }
    };

    return show ? (
        <div className={`flex items-center justify-center ${className || ''}`} style={{ height: '100%' }}>
            <form onSubmit={handleSubmit} className="w-full max-w-lg font-medium text-lg flex items-center justify-center">
                <div className="relative flex items-center justify-center w-full">
                    <textarea
                        id="name"
                        value={name}
                        ref={textareaRef}
                        className={`focus:outline-none cursor-pointer w-full bg-transparent border-2 border-transparent focus:border-black rounded-md transition duration-300 p-2 ${todo && todo.Done ? 'line-through opacity-50' : ''}`}
                        onChange={handleChange}
                        onKeyDown={handleKeyDown}
                        rows={1}
                        cols={30}
                        disabled={disableEdit}
                        placeholder={showPlaceholder ? "Enter todo item here..." : undefined}
                        style={{
                            resize: 'none',
                            overflow: 'hidden',
                            lineHeight: '1.5em',
                            cursor: disableEdit ? 'default' : 'pointer',
                        }}
                    />
                    {error && <p className="text-red-500 text-sm mt-2">{error}</p>}
                </div>
            </form>
        </div>
    ) : null;
};

export default TodoForm;
