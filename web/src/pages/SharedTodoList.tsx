import React, { useEffect, useState } from 'react';
import { fetchSharedApi } from "../features/todo/todoAPI";
import CollapsibleTodoList from '../features/todo/CollapsibleTodoList';
import CollapsibleButton from '../features/todo/CollapsibleButton';
import InfoBanner from '../features/todo/InfoBanner'; // Import the InfoBanner component

interface Todo {
    Id: string;
    Name: string;
    Done: boolean;
    Children?: Todo[];
    Deep: number; // Depth level
}

interface SharedTodo {
    sharedByUserEmail: string;
    todo: Todo;
}

const SharedTodoList: React.FC = () => {
    const [sharedTodos, setSharedTodos] = useState<SharedTodo[]>([]);
    const [collapsedTodoIds, setCollapsedTodoIds] = useState<Set<string>>(new Set());
    const [collapsedEmailIds, setCollapsedEmailIds] = useState<Set<string>>(new Set());

    useEffect(() => {
        const fetchSharedTodos = async () => {
            try {
                const data = await fetchSharedApi();
                const formattedTodos: SharedTodo[] = [];

                for (const [email, todos] of Object.entries(data)) {
                    todos.forEach(todo => {
                        formattedTodos.push({ sharedByUserEmail: email, todo });
                    });
                }

                setSharedTodos(formattedTodos);
            } catch (error) {
                console.error('Error fetching todos:', error);
            }
        };

        fetchSharedTodos();
    }, []);

    const toggleCollapse = (id: string) => {
        setCollapsedTodoIds(prev => {
            const newCollapsedIds = new Set(prev);
            if (newCollapsedIds.has(id)) {
                newCollapsedIds.delete(id);
            } else {
                newCollapsedIds.add(id);
            }
            return newCollapsedIds;
        });
    };

    const toggleEmailCollapse = (email: string) => {
        setCollapsedEmailIds(prev => {
            const newCollapsedIds = new Set(prev);
            if (newCollapsedIds.has(email)) {
                newCollapsedIds.delete(email);
            } else {
                newCollapsedIds.add(email);
            }
            return newCollapsedIds;
        });
    };

    const renderTodo = (todo: Todo, depth: number = 1) => (
        <CollapsibleTodoList
            key={todo.Id}
            todos={[todo]} // We pass an array with one todo
            collapsedTodoIds={collapsedTodoIds}
            toggleCollapse={toggleCollapse}
            fetchTodos={() => {}}
            disableActions={true}
            disableEdit={true}
        />
    );

    return (
        <div className="flex flex-col items-center p-4 font-custom" style={{ minHeight: 'calc(83vh)' }}>
            <div className="mb-4">
                <h2 className=" title-header text-3xl font-bold font-custom text-center mt-0 mb-5">Shared Todo List</h2>
                <InfoBanner /> {/* Include the InfoBanner component */}
            </div>
            <div className="w-full flex justify-end">
                <div className="overflow-x-auto w-full max-w-5xl ml-4"> {/* Adjust margin-left as needed */}
                    {sharedTodos.reduce((acc: JSX.Element[], st, index) => {
                        if (index === 0 || st.sharedByUserEmail !== sharedTodos[index - 1].sharedByUserEmail) {
                            const email = st.sharedByUserEmail;
                            acc.push(
                                <div key={email} className="font-custom mb-4">
                                    <div className="flex items-center mb-2">
                                        <CollapsibleButton
                                            id={email}
                                            isCollapsed={collapsedEmailIds.has(email)}
                                            hasChildren={true} // Always show button for emails
                                            onToggle={toggleEmailCollapse}
                                        />
                                        <h3 className="text-2xl font-semibold font-custom ml-2">Todolists from {email}</h3>
                                    </div>
                                    {!collapsedEmailIds.has(email) && (
                                        <div className="hierarchy-line pl-4">
                                            {sharedTodos
                                                .filter(todo => todo.sharedByUserEmail === email)
                                                .map(st => renderTodo(st.todo))}
                                        </div>
                                    )}
                                </div>
                            );
                        }
                        return acc;
                    }, [])}
                </div>
            </div>
        </div>
    );
};

export default SharedTodoList;
