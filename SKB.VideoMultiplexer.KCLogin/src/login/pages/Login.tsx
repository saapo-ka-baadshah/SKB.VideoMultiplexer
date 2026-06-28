import { useState } from "react";
import { kcSanitize } from "keycloakify/lib/kcSanitize";
import { useIsPasswordRevealed } from "keycloakify/tools/useIsPasswordRevealed";
import type { PageProps } from "keycloakify/login/pages/PageProps";
import { useScript } from "keycloakify/login/pages/Login.useScript";
import type { KcContext } from "../KcContext";
import type { I18n } from "../i18n";
import "./Login.css";

export default function Login(props: PageProps<Extract<KcContext, { pageId: "login.ftl" }>, I18n>) {
    const { kcContext, i18n, Template, classes } = props;
    const { social, realm, url, usernameHidden, login, auth, registrationDisabled, messagesPerField, message, enableWebAuthnConditionalUI, authenticators } =
        kcContext;
    const { msg, msgStr } = i18n;

    const [isLoginButtonDisabled, setIsLoginButtonDisabled] = useState(false);

    const webAuthnButtonId = "authenticateWebAuthnButton";

    useScript({
        webAuthnButtonId,
        kcContext,
        i18n
    });

    const alertClass =
        message?.type === "error"
            ? "kcAlertError"
            : message?.type === "success"
                ? "kcAlertSuccess"
                : message?.type === "warning"
                    ? "kcAlertWarning"
                    : message?.type === "info"
                        ? "kcAlertInfo"
                        : "";

    return (
        <Template
            kcContext={kcContext}
            i18n={i18n}
            doUseDefaultCss={false}
            classes={classes}
            displayMessage={false}
            headerNode={
                <div className="kcLoginTitle">{realm.displayName || msg("loginAccountTitle")}</div>
            }
            displayInfo={realm.password && realm.registrationAllowed && !registrationDisabled}
            infoNode={
                <div className="kcRegistration">
                    <span>
                        {msg("noAccount")}{" "}
                        <a tabIndex={8} href={url.registrationUrl}>
                            {msg("doRegister")}
                        </a>
                    </span>
                </div>
            }
            socialProvidersNode={
                realm.password && social?.providers !== undefined && social.providers.length !== 0 ? (
                    <div className="kcSocialSection">
                        <div className="kcSocialDivider">{msg("identity-provider-login-label")}</div>
                        <ul className="kcSocialProviders">
                            {social.providers.map((p) => (
                                <li key={p.alias} className="kcSocialProviderItem">
                                    <a
                                        id={`social-${p.alias}`}
                                        className="kcSocialProviderButton"
                                        href={p.loginUrl}
                                    >
                                        {p.iconClasses && (
                                            <i className={`kcSocialProviderIcon ${p.iconClasses}`} aria-hidden="true" />
                                        )}
                                        <span dangerouslySetInnerHTML={{ __html: kcSanitize(p.displayName) }} />
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </div>
                ) : undefined
            }
        >
            <div className="kcLoginCard">
                {message && (
                    <div className={`kcAlert ${alertClass}`}>
                        <span dangerouslySetInnerHTML={{ __html: kcSanitize(message.summary) }} />
                    </div>
                )}

                {realm.password && (
                    <form
                        id="kc-form-login"
                        onSubmit={() => {
                            setIsLoginButtonDisabled(true);
                            return true;
                        }}
                        action={url.loginAction}
                        method="post"
                    >
                        {!usernameHidden && (
                            <div className="kcFormGroup">
                                <label htmlFor="username" className="kcLabel">
                                    {!realm.loginWithEmailAllowed
                                        ? msg("username")
                                        : !realm.registrationEmailAsUsername
                                            ? msg("usernameOrEmail")
                                            : msg("email")}
                                </label>
                                <div className="input-with-icon-wrapper">
                                    <div className="input-icon-left">
                                        <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                                            <circle cx="12" cy="7" r="4" />
                                        </svg>
                                    </div>
                                    <input
                                        tabIndex={2}
                                        id="username"
                                        className={`kcInput ${messagesPerField.existsError("username", "password") ? "inputError" : ""}`}
                                        name="username"
                                        defaultValue={login.username ?? ""}
                                        type="text"
                                        autoFocus
                                        placeholder={
                                            !realm.loginWithEmailAllowed
                                                ? "Enter your Username"
                                                : !realm.registrationEmailAsUsername
                                                    ? "Enter your Username or Email"
                                                    : "Enter your Email"
                                        }
                                        autoComplete={enableWebAuthnConditionalUI ? "username webauthn" : "username"}
                                    />
                                </div>
                                {messagesPerField.existsError("username", "password") && (
                                    <span
                                        className="kcInputErrorMessage"
                                        aria-live="polite"
                                        dangerouslySetInnerHTML={{
                                            __html: kcSanitize(messagesPerField.getFirstError("username", "password"))
                                        }}
                                    />
                                )}
                            </div>
                        )}

                        <div className="kcFormGroup">
                            <label htmlFor="password" className="kcLabel">
                                {msg("password")}
                            </label>
                            <PasswordWrapper i18n={i18n} passwordInputId="password">
                                <input
                                    tabIndex={3}
                                    id="password"
                                    className={`kcInput ${messagesPerField.existsError("username", "password") ? "inputError" : ""}`}
                                    name="password"
                                    type="password"
                                    placeholder="Enter your password"
                                    autoComplete="current-password"
                                />
                            </PasswordWrapper>
                            {usernameHidden && messagesPerField.existsError("username", "password") && (
                                <span
                                    className="kcInputErrorMessage"
                                    aria-live="polite"
                                    dangerouslySetInnerHTML={{
                                        __html: kcSanitize(messagesPerField.getFirstError("username", "password"))
                                    }}
                                />
                            )}
                        </div>

                        <div className="kcFormSetting">
                            <div className="kcCheckbox">
                                {realm.rememberMe && !usernameHidden && (
                                    <label>
                                        <input
                                            tabIndex={5}
                                            id="rememberMe"
                                            name="rememberMe"
                                            type="checkbox"
                                            defaultChecked={!!login.rememberMe}
                                        />
                                        {msg("rememberMe")}
                                    </label>
                                )}
                            </div>
                            <div className="kcForgotPassword">
                                {realm.resetPasswordAllowed && (
                                    <a tabIndex={6} href={url.loginResetCredentialsUrl}>
                                        {msg("doForgotPassword")}
                                    </a>
                                )}
                            </div>
                        </div>

                        <div id="kc-form-buttons">
                            <input type="hidden" id="id-hidden-input" name="credentialId" value={auth.selectedCredential} />
                            <input
                                tabIndex={7}
                                disabled={isLoginButtonDisabled}
                                className="kcSubmitButton"
                                name="login"
                                id="kc-login"
                                type="submit"
                                value={msgStr("doLogIn")}
                            />
                        </div>
                    </form>
                )}

                {enableWebAuthnConditionalUI && (
                    <>
                        <form id="webauth" action={url.loginAction} method="post">
                            <input type="hidden" id="clientDataJSON" name="clientDataJSON" />
                            <input type="hidden" id="authenticatorData" name="authenticatorData" />
                            <input type="hidden" id="signature" name="signature" />
                            <input type="hidden" id="credentialId" name="credentialId" />
                            <input type="hidden" id="userHandle" name="userHandle" />
                            <input type="hidden" id="error" name="error" />
                        </form>

                        {authenticators !== undefined && authenticators.authenticators.length !== 0 && (
                            <form id="authn_select">
                                {authenticators.authenticators.map((authenticator, i) => (
                                    <input key={i} type="hidden" name="authn_use_chk" readOnly value={authenticator.credentialId} />
                                ))}
                            </form>
                        )}

                        <input
                            id={webAuthnButtonId}
                            type="button"
                            className="kcPasskeyButton"
                            value={msgStr("passkey-doAuthenticate")}
                        />
                    </>
                )}
            </div>
        </Template>
    );
}

function PasswordWrapper(props: { i18n: I18n; passwordInputId: string; children: JSX.Element }) {
    const { i18n, passwordInputId, children } = props;
    const { msgStr } = i18n;
    const { isPasswordRevealed, toggleIsPasswordRevealed } = useIsPasswordRevealed({ passwordInputId });

    return (
        <div className="input-with-icon-wrapper password-input-wrapper">
            <div className="input-icon-left">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                </svg>
            </div>
            {children}
            <button
                type="button"
                className="kcPasswordVisibilityButton"
                aria-label={msgStr(isPasswordRevealed ? "hidePassword" : "showPassword")}
                aria-controls={passwordInputId}
                onClick={toggleIsPasswordRevealed}
            >
                {isPasswordRevealed ? (
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                        <circle cx="12" cy="12" r="3" />
                    </svg>
                ) : (
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24" />
                        <line x1="1" y1="1" x2="23" y2="23" />
                    </svg>
                )}
            </button>
        </div>
    );
}
