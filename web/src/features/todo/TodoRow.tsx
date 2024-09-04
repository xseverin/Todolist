import React, { useState, useEffect, useRef } from 'react';
import { deleteTodoApi, toggleTodoApi, shareTodoApi } from './todoAPI.ts';
import TodoForm from './TodoForm';
import { App } from 'antd';
import deleteIcon from '../../img/delete.png';
import shareIcon from '../../img/share.png';
import CollapsibleButton from './CollapsibleButton';
import { Todo } from './Todo';
import CheckboxWithIcon from "./CheckBoxWithIcon.tsx";

interface TodoRowProps {
    todo?: Todo;
    fetchTodos: () => void;
    collapsedTodoIds?: Set<string>;
    toggleCollapse?: (id: string) => void;
    disableActions?: boolean;
    disableEdit?: boolean;
    parentId?: string;
    shouldHideForm?: boolean;
    showPlaceholder?: boolean;
}

const TodoRow: React.FC<TodoRowProps> = ({
                                             todo,
                                             fetchTodos,
                                             collapsedTodoIds,
                                             toggleCollapse,
                                             disableEdit = false,
                                             parentId,
                                             shouldHideForm,
                                             disableActions
                                         }) => {
    const [isMainFormVisible, setIsMainFormVisible] = useState(true);
    const [isSharing, setIsSharing] = useState(false);
    const [email, setEmail] = useState<string>('');
    const [isFormVisible, setIsFormVisible] = useState(false);
    const { message } = App.useApp();
    const [isHovered, setIsHovered] = useState(false);

    const formTimeout = useRef<NodeJS.Timeout | null>(null);
    const shareFormTimeout = useRef<NodeJS.Timeout | null>(null);
    const shareButtonRef = useRef<HTMLButtonElement | null>(null);
    const fillerDivRef = useRef<HTMLDivElement | null>(null);
    const todoFormRef = useRef<HTMLDivElement | null>(null);
    const buttonSpanRef = useRef<HTMLSpanElement | null>(null);
    const secondShareButtonRef = useRef<HTMLButtonElement | null>(null); // Ref for the second share button

    const updateFillerHeight = (newHeight?: number) => {
        if (fillerDivRef.current) {
            fillerDivRef.current.style.height = `${newHeight || fillerDivRef.current.offsetHeight}px`;
        }
    };

    const updateButtonSpanHeight = (newHeight?: number) => {
        if (buttonSpanRef.current) {
            buttonSpanRef.current.style.height = `${newHeight || buttonSpanRef.current.offsetHeight}px`;
        }
    };

    const toggleDone = async () => {
        if (todo) {
            await toggleTodoApi(todo.Id, todo.Done);
            fetchTodos();
        }
    };

    const deleteTodo = async () => {
        if (todo) {
            await deleteTodoApi(todo.Id);
            fetchTodos();
        }
    };

    const toggleSharing = () => {
        if (todo) {
            setIsSharing((prev) => !prev);
        }
    };

    const shareTodo = async () => {
        if (todo && email) {
            try {
                await shareTodoApi(email, todo.Id);
                setIsSharing(false);
                message.success('Todo shared successfully!');
            } catch {
                message.error('Todo sharing failed');
            }
        } else {
            alert('Please enter an email address.');
        }
    };

    const handleMouseEnter = () => {
        setIsHovered(true);
        if (formTimeout.current) {
            clearTimeout(formTimeout.current);
        }
        if (shareFormTimeout.current) {
            clearTimeout(shareFormTimeout.current);
        }
    };

    const handleMouseLeave = () => {
        if (isFormVisible) {
            formTimeout.current = setTimeout(() => {
                setIsFormVisible(false);
            }, 10000);
        }
        if (isSharing) {
            shareFormTimeout.current = setTimeout(() => {
                setIsSharing(false);
            }, 5000);
        }
        setIsHovered(false);
    };

    const handleAddTodoClick = () => {
        setIsFormVisible(true);
    };

    useEffect(() => {
        if (secondShareButtonRef.current) {
            const img = secondShareButtonRef.current.querySelector('img');
            if (img) {
                img.style.width = '60px';
                img.style.height = '20px'; // Ensure height is consistent
            }
        }

        return () => {
            if (formTimeout.current) {
                clearTimeout(formTimeout.current);
            }
            if (shareFormTimeout.current) {
                clearTimeout(shareFormTimeout.current);
            }
        };
    }, []);

    const handleHeightChange = (newHeight: number) => {
        updateFillerHeight(newHeight);
        updateButtonSpanHeight(newHeight);
    };

    return isMainFormVisible ? (
        <>
            <div
                onMouseEnter={handleMouseEnter}
                onMouseLeave={handleMouseLeave}
                className={`relative font-medium text-lg cursor-pointer flex items-center`}
            >
                <div className="flex items-center space-x-3 flex-shrink-0">
                    <CollapsibleButton
                        id={todo ? todo.Id : '11'}
                        isCollapsed={collapsedTodoIds && todo ? collapsedTodoIds.has(todo.Id) : false}
                        hasChildren={todo && todo.Children ? todo.Children.length > 0 : false}
                        onToggle={toggleCollapse ? toggleCollapse : () => {}}
                    />
                    <div
                        ref={todoFormRef}
                        className={`flex-grow flex items-center space-x-3 transition-all duration-300 ${isHovered ? 'hovered-dark' : ''}`}
                    >
                        <CheckboxWithIcon
                            isChecked={todo ? todo.Done : false}
                            isDisabled={todo === undefined}
                            onChange={toggleDone}
                        />
                        <TodoForm
                            todo={todo}
                            disableEdit={disableEdit}
                            fetchTodos={fetchTodos}
                            parentId={parentId}
                            shouldHideForm={shouldHideForm}
                            onVisibilityChange={setIsMainFormVisible}
                            showPlaceholder={true}
                            onHeightChange={handleHeightChange}
                        />
                    </div>
                </div>

                {/* Space filler with dynamic height */}
                <div
                    ref={fillerDivRef}
                    className={`flex-grow transition-all duration-300 ${isHovered ? 'hovered-dark' : ''}`}
                    style={{ width: '100%' }}
                ></div>

                <div className={`flex items-center space-x-3 flex-shrink-0`}>
                    <span
                        ref={buttonSpanRef}
                        className={`flex items-center justify-center transition-bg duration-300 ${isHovered ? 'hovered-dark' : ''}`}
                        style={{ width: '100%' }}
                    >
                        <button
                            className={`btn btn-primary pb-1 transition-opacity duration-300 pr-5 ${todo && todo.Deep !== undefined && !disableActions && todo.Deep < 2 && todo.Children?.length === 0 && isHovered ? 'opacity-100' : 'opacity-0'}`}
                            onClick={handleAddTodoClick}
                            disabled={todo === undefined}
                        >
                            +
                        </button>
                        <button
                            className={`btn btn-danger transition-opacity duration-300 pr-5 ${isHovered && todo !== undefined && !disableActions ? 'opacity-100' : 'opacity-0'}`}
                            onClick={deleteTodo}
                            disabled={todo === undefined}
                        >
                            <img src={deleteIcon} alt="Delete" />
                        </button>
                        <button
                            ref={shareButtonRef}
                            className={`btn btn-secondary w-[20px] transition-opacity duration-300  ${isHovered && !isSharing && todo !== undefined && !disableActions ? 'opacity-100' : 'opacity-0'}`}
                            onClick={toggleSharing}
                            disabled={todo === undefined}
                        >
                            <img src={shareIcon} alt="Share" />
                        </button>
                    </span>
                    <input
                        type="email"
                        placeholder="Enter email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className={`border border-gray-400 bg-transparent ${isSharing && isHovered ? 'opacity-100' : 'opacity-0'}`}
                    />
                    <button
                        ref={secondShareButtonRef} // Assign ref to second share button
                        className={`btn btn-secondary  ${isSharing && isHovered ? 'opacity-100' : 'opacity-0'}`}
                        onClick={shareTodo}
                    >
                        <img src={shareIcon} alt="Share" />
                    </button>
                </div>
            </div>

            {isFormVisible && (
                <div className="pl-8">
                    <TodoRow parentId={todo && todo.Id} shouldHideForm={true} fetchTodos={fetchTodos} />
                </div>
            )}
        </>
    ) : null;
};

export default TodoRow;
